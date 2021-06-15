using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AuditLog.API.Configuration;
using AuditLog.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AuditLog.API
{
    public class Startup
    {
        private const string SwaggerPath = "apidocs";

        private readonly string _assemblyName;

        private IServiceCollection? _services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? GetType().Namespace
                ?? string.Empty;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.RegisterOptions(Configuration);
            
            services
                .AddControllers()
                .AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _assemblyName, Version = "v1" });
                AddSwaggerXml(c, _assemblyName);
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureHealthCheckPipeline(app);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger(c => c.RouteTemplate = $"{SwaggerPath}/{{documentname}}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{SwaggerPath}/v1/swagger.json", $"{_assemblyName} v1");
                c.RoutePrefix = SwaggerPath;
                c.DocExpansion(DocExpansion.None);
            });

            app.UseCancellationTokenMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

#pragma warning disable CA1062
            WarmUp(app.ApplicationServices);
#pragma warning restore CA1062
        }

        private static void AddSwaggerXml(SwaggerGenOptions options, string assemblyName)
        {
            var xmlFileName = $"{assemblyName}.xml";
            var xmlFile = Directory.GetFiles(AppContext.BaseDirectory, xmlFileName).FirstOrDefault();
            if (xmlFile != null)
            {
                options.IncludeXmlComments(xmlFile);
            }
        }

        private void ConfigureHealthCheckPipeline(IApplicationBuilder app)
        {
            app.MapWhen(
                ctx => ctx.Request.Method == HttpMethod.Get.Method &&
                       ctx.Request.Path.StartsWithSegments("/health", StringComparison.Ordinal),
                builder =>
                    builder
                        .UseRouting()
                        .UseEndpoints(endpoints =>
                            endpoints.MapHealthChecks("/health").RequireHost($"*:{Configuration["ManagementPort"]}"))
            );
        }

        private void WarmUp(IServiceProvider serviceProvider)
        {
            var controllersList = (_services ?? throw new InvalidOperationException($"Field {nameof(_services)} must be initialized"))
                .Where(x => x.ServiceType.IsSubclassOf(typeof(ControllerBase)) && !x.ServiceType.IsAbstract)
                .Select(x => x.ServiceType);

            foreach (var controller in controllersList)
            {
                serviceProvider.GetService(controller);
            }
        }
    }
}
