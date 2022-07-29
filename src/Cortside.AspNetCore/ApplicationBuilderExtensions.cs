using System.Threading.Tasks;
using Cortside.Common.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Cortside.AspNetCore {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseApiDefaults(this IApplicationBuilder app, IConfiguration config) {
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseExceptionHandler(error => error.Run(_ => Task.CompletedTask));
            app.UseResponseCompression();
            app.UseResponseCaching();
            app.UseCors(builder => builder
                .WithOrigins(config.GetSection("Cors").GetSection("Origins").Get<string[]>())
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseSerilogRequestLogging();

            return app;
        }
    }
}
