using AuditLog.Data.MySql.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.API.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration.GetConnectionString("Default"));
            services.RegisterRepositories();
            
            return services;
        }
    }
}
