using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cortside.Bowdlerizer;
using Cortside.Health.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Bowdlerizer;

namespace Cortside.AspNetCore.Builder {
    public class WebApiBuilder {
        private IWebApiStartup startup;
        private IConfiguration config;
        private readonly string[] args;
        private WebApplication webApplication;
        private Bowdlerizer.Bowdlerizer bowdlerizer;
        private LoggerConfiguration loggerConfiguration;
        private string service;
        private bool executingIsEntryAssembly;
        private string url;

        public WebApiBuilder(string[] args) {
            this.args = args;
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

        public bool ExecutingIsEntryAssembly => executingIsEntryAssembly;

        public string Service => service;
        public string Url => url;

        public WebApplication WebApplication => webApplication;

        private void CreateWebApplication() {
            url = System.Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog(Log.Logger);

            builder.WebHost.ConfigureAppConfiguration(b => b.AddConfiguration(config));
            builder.WebHost.UseConfiguration(config);
            builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(10));

            builder.WebHost.UseKestrel();
            builder.WebHost.ConfigureKestrel(options => {
                options.ConfigureEndpointDefaults(listenOptions => {
                });
                options.AddServerHeader = false;
                options.Limits.MaxRequestLineSize = int.MaxValue;
                options.Limits.MaxRequestBufferSize = int.MaxValue;
            });
            builder.WebHost.UseDefaultServiceProvider(options => options.ValidateScopes = false);

            builder.Services.AddSingleton(bowdlerizer);

            startup?.ConfigureServices(builder.Services);

            var app = builder.Build();

            var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
            startup?.Configure(app, app.Environment, provider);

            app.Logger.LogInformation($"Service {app.Environment.ApplicationName} started with environment {app.Environment.EnvironmentName}");
            app.Logger.LogInformation($"args: [{string.Join(",", args)}]");
            app.Logger.LogInformation($"ASPNETCORE_URLS: {url}");
            app.Lifetime.ApplicationStarted.Register(() => LogAddresses(app.Services, app.Logger));

            webApplication = app;
        }

        static void LogAddresses(IServiceProvider services, Microsoft.Extensions.Logging.ILogger logger) {
            var server = services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            foreach (var address in addressFeature.Addresses) {
                logger.LogInformation($"Listing on address: {address}");
            }
        }

        public IServiceCollection Services { get; }
        public ILoggingBuilder Logging { get; }
        public ConfigureWebHostBuilder WebHost { get; }
        public ConfigureHostBuilder Host { get; }

        public WebApiBuilder UseConfiguration() {
            config = GetConfiguration();
            return this;
        }

        public WebApiBuilder UseConfiguration(IConfiguration config) {
            this.config = config;
            return this;
        }

        public WebApiBuilder UseStartup<TStartup>() where TStartup : IWebApiStartup, new() {
            startup = new TStartup();
            return this;
        }

        public WebApi Build() {
            config ??= GetConfiguration();

            startup?.UseConfiguration(config);

            var build = config.GetSection("Build").Get<BuildModel>();
            service = Assembly.GetEntryAssembly().GetName().FullName;
            executingIsEntryAssembly = GetExecutingIsEntryAssembly();

            var rules = config.GetSection("Bowdlerizer").Get<List<BowdlerizerRuleConfiguration>>();
            bowdlerizer = new Bowdlerizer.Bowdlerizer(rules);

            loggerConfiguration ??= GetLoggerConfiguration(build, service, bowdlerizer);
            Log.Logger = loggerConfiguration.CreateLogger();

            CreateWebApplication();
            return new WebApi(this);
        }

        private static bool GetExecutingIsEntryAssembly() {
            var entry = Assembly.GetEntryAssembly();
            var calling = Assembly.GetCallingAssembly();
            var executing = Assembly.GetExecutingAssembly();

            return calling == entry && entry == executing;
        }

        private LoggerConfiguration GetLoggerConfiguration(BuildModel build, string service, Bowdlerizer.Bowdlerizer bowdlerizer) {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", Environment)
                .Enrich.WithProperty("Service", service)
                .Enrich.WithProperty("BuildVersion", build.Version)
                .Enrich.WithProperty("BuildTag", build.Tag)
                .Enrich.WithBowdlerizer(bowdlerizer);

            var serverUrl = config["Seq:ServerUrl"];
            if (!string.IsNullOrWhiteSpace(serverUrl)) {
                loggerConfiguration.WriteTo.Seq(serverUrl);
            }
            var logFile = config["LogFile:Path"];
            if (!string.IsNullOrWhiteSpace(logFile)) {
                loggerConfiguration.WriteTo.File(logFile);
            }

            return loggerConfiguration;
        }
    }
}
