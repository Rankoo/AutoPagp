using AutoAlertBackEnd;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExternal(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AutoAlerBack", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme <br /> <br />
                        Enter Bearer [space] and then your token in the text input bellow <br /> <br />
                        Example: 'Bearer 123456abcdefg' <br /> <br />.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    // OAuth2 implicit/authorization code flow for Google
    //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    //{
    //    Type = SecuritySchemeType.OAuth2,
    //    Flows = new OpenApiOAuthFlows
    //    {
    //        AuthorizationCode = new OpenApiOAuthFlow
    //        {
    //            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
    //            TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
    //            Scopes = new Dictionary<string, string>
    //            {
    //                { "openid", "OpenID" },
    //                { "profile", "Profile" },
    //                { "email", "Email" }
    //            }
    //        }
    //    }
    //});
    // Require oauth2 for endpoints when used
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
    //        },
    //        new[] { "openid", "profile", "email" }
    //    }
    //});
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyOrigin()  // Permitir cualquier origen
            .AllowAnyMethod()  // Permitir cualquier m�todo HTTP
            .AllowAnyHeader()); // Permitir cualquier cabecera
});
// Validate JWT config early to fail fast and avoid nullable warnings
var jwtKey = builder.Configuration["Jwt:Key"];
var google = builder.Configuration.GetSection("Authentication:Google");

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("Configuration value 'Jwt:Key' is missing or empty. Please set it in appsettings.");
}

var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
//.AddCookie()
//.AddGoogle(options =>
//{
//   options.ClientId = google["ClientId"]!;
//   options.ClientSecret = google["ClientSecret"]!;
//   options.CallbackPath = "/signin-google";
//   options.SaveTokens = true;

//   options.Events.OnCreatingTicket= async ctx =>
//   {

//        var services = ctx.HttpContext.RequestServices;
//        var configuration = services.GetRequiredService<IConfiguration>();
//        var userRepo = services.GetRequiredService<AutoAlertBackEnd.Repositories.IUserRepository>();

//        var email = ctx.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
//        if (string.IsNullOrEmpty(email))
//        {
//            // cannot proceed without email
//            return;
//        }
//        var user = await userRepo.GetUserByEmailAsync(email);

//        if (user == null)
//        {
//            Console.WriteLine("Crea el usuario");
//        }
//        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing"));
//        var claims = new List<System.Security.Claims.Claim>
//        {
//            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
//            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, email ?? string.Empty),
//        };
//        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new System.Security.Claims.ClaimsIdentity(claims),
//            Expires = DateTime.UtcNow.AddHours(8),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//            Issuer = configuration["Jwt:Issuer"],
//            Audience = configuration["Jwt:Audience"]
//        };
//        var jwt = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

//        // Redirige al frontend con el token en query (sólo para pruebas; en producción usa POST o cookie segura)
//        var redirectUrl = ctx.Properties.RedirectUri ?? "/";
//        var urlWithToken = $"{redirectUrl}?token={Uri.EscapeDataString(jwt)}";
//        ctx.Response.Redirect(urlWithToken);
//   };
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Configure Swagger UI to use Google OAuth for authorization code flow (PKCE)
        c.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthAppName("AutoAlert - Google OAuth");
    });
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(
            new { message = "Acceso no autorizado, verifique sus credenciales." }
        );
        await context.Response.WriteAsync( result );
    }
});
app.UseAuthorization();
app.MapControllers();
app.Run();