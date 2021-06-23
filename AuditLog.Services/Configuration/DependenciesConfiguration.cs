using AuditLog.Data.MySql.Configuration;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.Services.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection RegisterProviders(this IServiceCollection services, string connectionString)
        {
            services.RegisterDbContext(connectionString);
            services.RegisterRepositories();

            services.AddTransient<IUsersProvider, UsersProvider>();
            services.AddTransient<IAuditLogsProvider, AuditLogsProvider>();
            
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
