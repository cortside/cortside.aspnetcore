using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Cortside.Bowdlerizer;
using Cortside.Health.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Bowdlerizer;

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
            var build = builder.Configuration.GetSection("Build").Get<BuildModel>();
            var service = Assembly.GetEntryAssembly().GetName();
            var executingIsEntryAssembly = ExecutingIsEntryAssembly();

            var rules = builder.Configuration.GetSection("Bowdlerizer").Get<List<BowdlerizerRuleConfiguration>>();
            var bowdlerizer = new Bowdlerizer.Bowdlerizer(rules);

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", Environment)
                .Enrich.WithProperty("Service", service)
                .Enrich.WithProperty("BuildVersion", build.Version)
                .Enrich.WithProperty("BuildTag", build.Tag)
                .Enrich.WithBowdlerizer(bowdlerizer);

            var serverUrl = builder.Configuration["Seq:ServerUrl"];
            if (!string.IsNullOrWhiteSpace(serverUrl)) {
                loggerConfiguration.WriteTo.Seq(serverUrl);
            }
            var logFile = builder.Configuration["LogFile:Path"];
            if (!string.IsNullOrWhiteSpace(logFile)) {
                loggerConfiguration.WriteTo.File(logFile);
            }
            Log.Logger = loggerConfiguration.CreateLogger();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            try {
                // don't log these when the entry assembly is not _this_ assembly, meaning that it's
                // being executed by something like dotnet ef for migrations
                if (executingIsEntryAssembly) {
                    Log.Information("Starting {Service}", service);
                    Log.Information("ASPNETCORE environment = {Environment}", Environment);
                }
                var appBuilder = WebApplication.CreateBuilder(builder.Args);
                appBuilder.Host.UseSerilog(Log.Logger);

                appBuilder.WebHost.ConfigureAppConfiguration(b => b.AddConfiguration(builder.Configuration));
                appBuilder.WebHost.UseConfiguration(builder.Configuration);
                appBuilder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(10));
                appBuilder.WebHost.ConfigureKestrel(options => {
                    options.AddServerHeader = false;
                    options.Limits.MaxRequestLineSize = int.MaxValue;
                    options.Limits.MaxRequestBufferSize = int.MaxValue;
                });
                appBuilder.WebHost.UseDefaultServiceProvider(options => options.ValidateScopes = false);

                appBuilder.Services.AddSingleton(bowdlerizer);

                builder.Startup.ConfigureServices(appBuilder.Services);

                var app = appBuilder.Build();

                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                builder.Startup.Configure(app, app.Environment, provider);

                // start host along with any startup tasks
                await app.RunWithTasksAsync().ConfigureAwait(false);

                return 0;
            } catch (Exception ex) when (ex is not OperationCanceledException && ex.GetType().Name != "StopTheHostException" && !executingIsEntryAssembly) {
                // FYI the exception filter is to handle dotnet ef commands when dealing with migrations
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        private static bool ExecutingIsEntryAssembly() {
            var entry = Assembly.GetEntryAssembly();
            var calling = Assembly.GetCallingAssembly();
            var executing = Assembly.GetExecutingAssembly();

            return calling == entry && entry == executing;
        }

        public static string Environment => System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static IConfiguration GetConfiguration() {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddJsonFile("build.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
