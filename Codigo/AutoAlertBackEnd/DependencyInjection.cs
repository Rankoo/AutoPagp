using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoAlertBackEnd.Context;
using AutoAlertBackEnd.Repositories;

namespace AutoAlertBackEnd
{
    public static class DependencyInjection
    {
        // Extensión para registrar servicios externos (ej. DbContext, clientes, etc.)
        public static IServiceCollection AddExternal(this IServiceCollection services, IConfiguration _configuration)
        {
            string connectionString = _configuration["ConnectionStrings:SQLConnectionStrings"] ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContext<AutoAlertContext>(opts => opts.UseSqlServer(connectionString));
            }

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}
