using AuditLog.Data.MySql.DbContext;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Data.MySql.Repositories;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.Data.MySql.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            LinqToDbSettings.SetUp();
            
            services.AddLinqToDbContext<AppDataConnection>((provider, builder) =>
            {
                builder
                    .UseMySqlConnector(connectionString)
                    .UseDefaultLogging(provider);
            }, ServiceLifetime.Scoped);
            
            return services;
        }
        
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            
            return services;
        }
    }
}
