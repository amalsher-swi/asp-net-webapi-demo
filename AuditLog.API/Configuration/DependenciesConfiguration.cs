using AuditLog.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.API.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterProviders(configuration.GetConnectionString("Default"));
            services.RegisterServices();
            
            return services;
        }
    }
}
