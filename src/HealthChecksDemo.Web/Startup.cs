using HealthChecks.UI.Client;
using HealthChecksDemo.Web.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthChecksDemo.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
                .AddCheck<CustomHealthCheck>("Custom health check");

            // Healthchecks UI
            services.AddHealthChecksUI(opt =>
            {
                opt.AddHealthCheckEndpoint("Demo web", "/health"); // Map health check API
            })
            .AddInMemoryStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Adding endpoints of health check for the health check UI in UI format
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                // Map healthcheck UI endpoint. Default is /healthchecks-ui
                endpoints.MapHealthChecksUI();

                endpoints.MapGet("/", async context => await context.Response.WriteAsync("Hello World!"));
            });
        }
    }
}