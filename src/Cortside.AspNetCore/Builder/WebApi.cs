using System;
using System.Globalization;
using System.Threading.Tasks;
using Serilog;

namespace Cortside.AspNetCore.Builder {
    /// <summary>
    /// Adapter class for WebApplication that will start up with common configuration
    /// </summary>
    public class WebApi {
        private readonly WebApiBuilder builder;

        public WebApi(WebApiBuilder builder) {
            this.builder = builder;
        }

        public static WebApiBuilder CreateBuilder() {
            return new WebApiBuilder(new string[] { });
        }

        public static WebApiBuilder CreateBuilder(string[] args) {
            return new WebApiBuilder(args);
        }

        /// <summary>
        /// Starts the webapi
        /// </summary>
        /// <param name="t">WebApiStartup</param>
        /// <param name="args">The arguments.</param>
        /// <param name="config">The configuration.</param>
        public async Task<int> StartAsync() {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            try {
                // don't log these when the entry assembly is not _this_ assembly, meaning that it's
                // being executed by something like dotnet ef for migrations
                if (builder.ExecutingIsEntryAssembly) {
                    Log.Information("Starting {Service}", builder.Service);
                    Log.Information("ASPNETCORE environment = {Environment}", WebApiBuilder.Environment);
                }

                var app = builder.WebApplication;

                // start host along with any startup tasks
                await app.RunWithTasksAsync().ConfigureAwait(false);

                return 0;
            } catch (Exception ex) when (ex is not OperationCanceledException && ex.GetType().Name != "StopTheHostException" && !builder.ExecutingIsEntryAssembly) {
                // FYI the exception filter is to handle dotnet ef commands when dealing with migrations
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }
    }
}
