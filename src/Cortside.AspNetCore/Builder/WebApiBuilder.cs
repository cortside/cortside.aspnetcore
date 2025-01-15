using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Cortside.AspNetCore.Enrichers;
using Cortside.Bowdlerizer;
using Cortside.Health.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
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

        private Action<IHostBuilder> hostConfigurationAction = null;
        private Action<IWebHostBuilder> webHostConfigurationAction = null;
        private Action<LoggerConfiguration> loggerConfigurationAction = null;

        public WebApiBuilder WithHostBuilder(Action<IHostBuilder> configuration) {
            this.hostConfigurationAction = configuration;
            return this;
        }

        public WebApiBuilder WithWebHostBuilder(Action<IWebHostBuilder> configuration) {
            this.webHostConfigurationAction = configuration;
            return this;
        }

        public WebApiBuilder WithLoggerConfiguration(Action<LoggerConfiguration> configuration) {
            this.loggerConfigurationAction = configuration;
            return this;
        }

        public WebApiBuilder(string[] args) {
            this.args = args;
        }

        public static string Environment => System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static IConfiguration GetConfiguration() {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddJsonFile("build.json", true, true)
                .AddEnvironmentVariables()
                // https://github.com/dotnet/aspnetcore/issues/37680#issuecomment-1331559463
                // This is the special line of code. It should be added in the place where you want to override configuration
                .AddTestConfiguration()
                .Build();

            config.ExpandTemplates();

            return config;
        }

        public bool ExecutingIsEntryAssembly => executingIsEntryAssembly;

        public string Service => service;
        public string Url => url;

        public WebApplication WebApplication => webApplication;

        private void CreateWebApplication(string certificateThumbprint = null) {
            url = System.Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

            var builder = WebApplication.CreateBuilder(args);

            if (hostConfigurationAction != null) {
                hostConfigurationAction(builder.Host);
            }

            if (webHostConfigurationAction != null) {
                webHostConfigurationAction(builder.WebHost);
            }

            builder.Host.UseSerilog(Log.Logger);

            builder.WebHost.ConfigureAppConfiguration(b => b.AddConfiguration(config));
            builder.WebHost.UseConfiguration(config);
            builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(10));

            builder.WebHost.UseKestrel(o => {
                o.AddServerHeader = false;
                if (!string.IsNullOrWhiteSpace(certificateThumbprint)) {
                    o.ListenAnyIP(new Uri(url).Port, listenOptions => {
                        listenOptions.UseHttps(X509.GetCertificate(certificateThumbprint));
                    });
                }
            });
            builder.WebHost.ConfigureKestrel(options => {
                options.ConfigureEndpointDefaults(_ => { });
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

        public WebApiHost Build() {
            // TODO: use bootstraplogger
            //https://nblumhardt.com/2020/10/bootstrap-logger/

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
            return new WebApiHost(this);
        }

        private static bool GetExecutingIsEntryAssembly() {
            var entry = Assembly.GetEntryAssembly();
            var calling = Assembly.GetCallingAssembly();
            var executing = Assembly.GetExecutingAssembly();

            return calling == entry && entry == executing;
        }

        private LoggerConfiguration GetLoggerConfiguration(BuildModel build, string serviceName, Bowdlerizer.Bowdlerizer bowdlerizer) {
            var configuration = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .UsingBowdlerizer(bowdlerizer)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", Environment)
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("BuildVersion", build.Version)
                .Enrich.WithProperty("BuildTag", build.Tag)
                .Enrich.With<OperationIdEnricher>();

            var serverUrl = config["Seq:ServerUrl"];
            if (!string.IsNullOrWhiteSpace(serverUrl)) {
                configuration.WriteTo.Seq(serverUrl);
            }
            var logFile = config["LogFile:Path"];
            if (!string.IsNullOrWhiteSpace(logFile)) {
                configuration.WriteTo.File(logFile);
            }

            if (loggerConfigurationAction != null) {
                loggerConfigurationAction(configuration);
            }

            return configuration;
        }
    }
}
