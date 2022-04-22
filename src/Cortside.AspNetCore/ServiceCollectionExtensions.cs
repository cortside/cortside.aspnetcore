using System;
using System.IO.Compression;
using Cortside.Common.BootStrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Adds a startup task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services) where T : class, IStartupTask {
            services.AddTransient<IStartupTask, T>();
            services.AddSingleton(services);
            return services;
        }

        public static IServiceCollection AddBootStrapper<T>(this IServiceCollection services, IConfiguration configuration, Action<BootStrapperOptions> options) where T : BootStrapper, new() {
            services.AddSingleton(configuration);
            var bootstrapper = new T();

            var o = new BootStrapperOptions();
            options?.Invoke(o);

            foreach (var installer in o.Installers) {
                bootstrapper.AddInstaller(installer);
            }
            bootstrapper.InitIoCContainer(configuration as IConfigurationRoot, services);
            return services;
        }

        public static IServiceCollection AddDefaultResponseCompression(this IServiceCollection services, CompressionLevel compressionLevel) {
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options => {
                options.Level = compressionLevel;
            });
            services.Configure<GzipCompressionProviderOptions>(options => {
                options.Level = compressionLevel;
            });

            return services;
        }
    }
}
