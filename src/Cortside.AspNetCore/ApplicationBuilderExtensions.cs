using System.Threading.Tasks;
using Cortside.AspNetCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Cortside.AspNetCore {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseApiDefaults(this IApplicationBuilder app, IConfiguration config) {
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<RequestIpAddressLoggingMiddleware>();
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

            // To handle x-forwarded-* headers
            // This would allow service to read the X-Forwarded-Proto header and reflect the actual scheme used by the client, even if it's behind a proxy.
            var fordwardedHeaderOptions = new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            return app;
        }
    }
}
