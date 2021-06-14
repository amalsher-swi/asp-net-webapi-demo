using AuditLog.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.API.Configuration
{
    public static class OptionsConfiguration
    {
        public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CancellationTokenOptions>(configuration.GetSection(nameof(CancellationTokenOptions)));

            return services;
        }
    }
}
