using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AuditLog.API.Configuration;
using AuditLog.API.Middleware;
using AuditLog.API.Models;
using AuditLog.Services.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            _assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? GetType().Namespace!;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.RegisterOptions(Configuration);

            services.AddAutoMapper(
                c => c.AddProfile<MappingConfiguration>(),
                typeof(Startup));

            services.RegisterDependencies(Configuration);

            ConfigureAuthentication(services);

            services
                .AddControllers()
                .AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _assemblyName, Version = "v1" });
                AddSwaggerXml(c, _assemblyName);
                AddSecurityRequirements(c);
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
            // else
            // {
            //     app.UseHsts();
            // }

            app.UseSwagger(c => c.RouteTemplate = $"{SwaggerPath}/{{documentname}}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{SwaggerPath}/v1/swagger.json", $"{_assemblyName} v1");
                c.RoutePrefix = SwaggerPath;
                c.DocExpansion(DocExpansion.None);
            });

            app.UseCancellationTokenMiddleware();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
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

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var authOptions = Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Url;
                    options.Audience = authOptions.Scopes;
                });
        }

        private void AddSecurityRequirements(SwaggerGenOptions options)
        {
            var authOptions = Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>();

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authOptions.Url}/connect/authorize"),
                        TokenUrl = new Uri($"{authOptions.Url}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { authOptions.Scopes, "Audit access" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "oauth2"
                        }
                    },
                    new[] { authOptions.Scopes }
                }
            });
        }

        private void WarmUp(IServiceProvider serviceProvider)
        {
            var controllersList = (_services ?? throw new InvalidOperationException($"Field {nameof(_services)} must be initialized"))
                .Where(x => x.ServiceType.IsSubclassOf(typeof(ControllerBase)) && !x.ServiceType.IsAbstract)
                .Select(x => x.ServiceType);

            using var scope = serviceProvider.CreateScope();

            foreach (var controller in controllersList)
            {
                scope.ServiceProvider.GetService(controller);
            }
        }
    }
}
