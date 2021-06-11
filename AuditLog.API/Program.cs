using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using AuditLog.API.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AuditLog.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration.ConfigureLogging();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "App terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.Configure(options =>
                    {
                        options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId | ActivityTrackingOptions.ParentId;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
    }
}
