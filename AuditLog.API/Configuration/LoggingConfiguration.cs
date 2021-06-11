using System;
using System.Globalization;
using System.Reflection;
using AuditLog.API.Helpers;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace AuditLog.API.Configuration
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

            // Configuration is not yet created, so we need to manually build it
            var configuration = GetConfiguration(environment);

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With<ActivityEnricher>() // https://github.com/serilog/serilog-aspnetcore/issues/207
                .WriteTo.Console()
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration);

            var elasticUri = configuration["ElasticSearchUrl"];
            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                loggerConfig
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat =
                            $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower(CultureInfo.CurrentCulture).Replace(".", "-", StringComparison.InvariantCulture)}-" +
                            $"{environment.ToLower(CultureInfo.CurrentCulture).Replace(".", "-", StringComparison.InvariantCulture)}-{DateTime.UtcNow:yyyy.MM.dd}",
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           EmitEventFailureHandling.WriteToFailureSink |
                                           EmitEventFailureHandling.RaiseCallback,
                        FailureSink = new SharedFileSink("./failures.txt", new JsonFormatter(), 100L * 1024 * 1024)
                    });
            }
            
            Log.Logger = loggerConfig.CreateLogger();
        }

        private static IConfiguration GetConfiguration(string environment)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
