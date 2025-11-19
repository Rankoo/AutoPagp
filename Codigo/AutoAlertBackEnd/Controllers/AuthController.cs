using AutoAlertBackEnd.Dtos;
using AutoAlertBackEnd.Models;
using AutoAlertBackEnd.Repositories;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoAlertBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        { 
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("logIn")]
        public async Task<IActionResult> LogIn(LogInDto logIn)
        {
            if (logIn == null || string.IsNullOrEmpty(logIn.Email) || string.IsNullOrEmpty(logIn.Password))
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userRepository.GetUserByEmailAsync(logIn.Email);
            if (user == null) { 
                return Unauthorized();
            }
            if (BCrypt.Net.BCrypt.Verify(logIn.Password, user.PasswordHash))
            {
                var token = GenerateJwtForUser(user);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        //[HttpPost("google-signin")]
        //public async Task<IActionResult> GoogleSignIn([FromBody] ExternalAuthDto dto)
        //{
        //    if (dto == null || string.IsNullOrEmpty(dto.IdToken))
        //        return BadRequest("IdToken is required.");

        //    // Validate Google id_token
        //    var clientId = _configuration["Authentication:Google:ClientId"];
        //    try
        //    {
        //        var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings
        //        {
        //            Audience = new[] { clientId }
        //        });

        //        var email = payload.Email;
        //        if (string.IsNullOrEmpty(email))
        //            return BadRequest("Google token does not contain email.");

        //        // Find or create local user
        //        var user = await _userRepository.GetUserByEmailAsync(email);
        //        if (user == null)
        //        {
        //            user = new AutoAlertBackEnd.Models.Users
        //            {
        //                Email = email,
        //                Names = payload.Name ?? payload.GivenName ?? "GoogleUser",
        //                GoogleId = payload.Subject,
        //                IsActive = true
        //            };
        //            user = await _userRepository.CreateUserAsync(user);
        //        }
        //        else if (string.IsNullOrEmpty(user.GoogleId))
        //        {
        //            user.GoogleId = payload.Subject;
        //            await _userRepository.UpdateUserAsync(user);
        //        }

        //        var token = GenerateJwtForUser(user);
        //        return Ok(new { Token = token });
        //    }
        //    catch (InvalidJwtException ex)
        //    {
        //        return BadRequest($"Invalid Google token: {ex.Message}");
        //    }
        //}

        private string GenerateJwtForUser(Users user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
            var issuer = _configuration["Jwt:Issuer"] ?? _configuration["Jwt:Iusser"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Names)
            };
            if (user.RoleId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.RoleId.Value.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
