using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Mime;
using Cortside.AspNetCore.Filters;
using Cortside.AspNetCore.ModelBinding;
using Cortside.Common.BootStrap;
using Cortside.Common.Cryptography;
using Cortside.Common.Json;
using Cortside.Common.Messages.Filters;
using Cortside.Common.Messages.Results;
using Cortside.Health.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
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
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask {
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

        public static IServiceCollection AddDefaultResponseCompression(this IServiceCollection services,
            CompressionLevel compressionLevel) {
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = compressionLevel; });
            services.Configure<GzipCompressionProviderOptions>(options => { options.Level = compressionLevel; });

            return services;
        }

        public static IMvcBuilder AddApiControllers<T>(this IServiceCollection services, InternalDateTimeHandling internalDateTimeHandling) where T : IActionFilter {
            return services.AddApiControllers(new List<Type>() { typeof(T) }, new List<IOutputFormatter>(), internalDateTimeHandling);
        }

        public static IMvcBuilder AddApiControllers(this IServiceCollection services, List<Type> filters, List<IOutputFormatter> outputFormatters, InternalDateTimeHandling internalDateTimeHandling) {
            var mvcBuilder = services.AddControllers(options => {
                options.CacheProfiles.Add("Default", new CacheProfile {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
                foreach (var filter in filters) {
                    options.Filters.Add(filter);
                }
                foreach (var formatter in outputFormatters) {
                    options.OutputFormatters.Add(formatter);
                }

                options.Conventions.Add(new ApiControllerVersionConvention());
                options.ModelBinderProviders.Insert(0, new UtcDateTimeModelBinderProvider(internalDateTimeHandling));
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

        public static IMvcBuilder AddApiDefaults<T>(this IServiceCollection services, InternalDateTimeHandling internalDateTimeHandling) where T : IActionFilter {
            return services.AddApiDefaults(new List<Type>() { typeof(T) }, internalDateTimeHandling);
        }

        public static IMvcBuilder AddApiDefaults(this IServiceCollection services, List<Type> filters, InternalDateTimeHandling internalDateTimeHandling) {
            return services.AddApiDefaults(filters, new List<IOutputFormatter>(), internalDateTimeHandling);
        }

        public static IMvcBuilder AddApiDefaults(this IServiceCollection services, List<Type> filters, List<IOutputFormatter> outputFormatters, InternalDateTimeHandling internalDateTimeHandling) {
            // add response compression using gzip and brotli compression
            services.AddDefaultResponseCompression(CompressionLevel.Optimal);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddOptions();
            var mvcBuilder = services.AddApiControllers(filters, outputFormatters, internalDateTimeHandling);

            // warm all the services up, can chain these together if needed
            services.AddStartupTask<WarmupServicesStartupTask>();

            return mvcBuilder;
        }

        public static IMvcBuilder AddApiDefaults(this IServiceCollection services, InternalDateTimeHandling internalDateTimeHandling = InternalDateTimeHandling.Utc) {
            var mvcBuilder = services.AddApiDefaults<MessageExceptionResponseFilter>(internalDateTimeHandling);

            return mvcBuilder;
        }

        public static IServiceCollection AddRestApiClient<TInterface, TImplementation, TConfiguration>(
            this IServiceCollection services, IConfiguration configuration, string key)
            where TImplementation : class, TInterface
            where TInterface : class
            where TConfiguration : class {
            var hartConfiguration = configuration.GetSection(key).Get<TConfiguration>();
            services.AddSingleton(hartConfiguration);
            services.AddTransient<TInterface, TImplementation>();

            return services;
        }

        public static IServiceCollection AddEncryptionService(this IServiceCollection services, string secret) {
            services.AddSingleton<IEncryptionService>(new EncryptionService(secret));

            return services;
        }
    }
}
