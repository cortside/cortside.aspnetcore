using System;
using System.IO.Compression;
using System.Net.Mime;
using Cortside.AspNetCore.Filters;
using Cortside.AspNetCore.Filters.Results;
using Cortside.AspNetCore.ModelBinding;
using Cortside.Common.BootStrap;
using Cortside.Common.Cryptography;
using Cortside.Common.Json;
using Cortside.Health.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
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

        public static IServiceCollection AddBootStrapper<T>(this IServiceCollection services,
            IConfiguration configuration, Action<BootStrapperOptions> options) where T : BootStrapper, new() {
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
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = compressionLevel; });
            services.Configure<GzipCompressionProviderOptions>(options => { options.Level = compressionLevel; });

            return services;
        }

        public static IMvcBuilder AddApiDefaults(this IServiceCollection services, InternalDateTimeHandling internalDateTimeHandling = InternalDateTimeHandling.Utc, Action<MvcOptions> mvcAction = null) {
            // add response compression using gzip and brotli compression
            services.AddDefaultResponseCompression(CompressionLevel.Optimal);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddOptions();

            var mvcBuilder = services.AddApiControllers(internalDateTimeHandling, mvcAction);

            // warm all the services up, can chain these together if needed
            services.AddStartupTask<WarmupServicesStartupTask>();

            return mvcBuilder;
        }

        public static IMvcBuilder AddApiControllers(this IServiceCollection services, InternalDateTimeHandling internalDateTimeHandling = InternalDateTimeHandling.Utc, Action<MvcOptions> mvcAction = null) {
            var mvcBuilder = services.AddControllers(options => {
                options.CacheProfiles.Add("Default", new CacheProfile {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
                options.SuppressAsyncSuffixInActionNames = false;
                options.Conventions.Add(new ApiControllerVersionConvention());
                options.ModelBinderProviders.Insert(0, new UtcDateTimeModelBinderProvider(internalDateTimeHandling));
                if (mvcAction == null) {
                    mvcAction = (o) => { o.Filters.Add<Filters.MessageExceptionResponseFilter>(); };
                }
                mvcAction.Invoke(options);
            });
            mvcBuilder.ConfigureApiBehaviorOptions(options => {
                options.InvalidModelStateResponseFactory = context => {
                    var result = new ValidationFailedResult(context.ModelState);
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    return result;
                };
            });
            mvcBuilder.AddNewtonsoftJson(options => {
                JsonNetUtility.ApplyGlobalDefaultSettings(options.SerializerSettings, internalDateTimeHandling);
                options.SerializerSettings.Converters.Add(new IsoTimeSpanConverter());
            });
            mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(HealthController).Assembly));

            services.AddRouting(options => { options.LowercaseUrls = true; });

            return mvcBuilder;
        }

        public static IServiceCollection AddEncryptionService(this IServiceCollection services, string secret) {
            services.AddSingleton<IEncryptionService>(new EncryptionService(secret));

            return services;
        }
    }
}
