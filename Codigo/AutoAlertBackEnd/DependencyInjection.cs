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
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ISubModuleRepository, SubModuleRepository>();
            services.AddScoped<IRoleSubModuleRepository, RoleSubModuleRepository>();
            services.AddScoped<IUserSubmoduleRepository, UserSubmoduleRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserCompanyRepository, UserCompanyRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}
