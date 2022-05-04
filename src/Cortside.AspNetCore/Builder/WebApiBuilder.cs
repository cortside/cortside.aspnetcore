using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cortside.AspNetCore.Builder {
    public class WebApiBuilder {
        private IWebApiStartup startup;
        private IConfiguration config;
        private string[] args;

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

        public IWebApiStartup Startup => startup;
        public IConfiguration Configuration => config;
        public string[] Args => args;

        public IServiceCollection Services { get; }
        public ILoggingBuilder Logging { get; }
        public ConfigureWebHostBuilder WebHost { get; }
        public ConfigureHostBuilder Host { get; }

        public WebApiBuilder UseConfiguration() {
            this.config = GetConfiguration();
            return this;
        }

        public WebApiBuilder UseConfiguration(IConfiguration config) {
            this.config = config;
            return this;
        }

        public WebApiBuilder UseStartup<TStartup>() where TStartup : IWebApiStartup, new() {
            this.startup = new TStartup();
            return this;
        }

        public WebApi Build() {
            if (startup != null) {
                startup.UseConfiguration(config);
            }
            return new WebApi(this);
        }
    }
}
