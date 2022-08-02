using System;
using System.IO.Compression;
using System.Net.Mime;
using Cortside.AspNetCore.Filters;
using Cortside.Common.BootStrap;
using Cortside.Common.Json;
using Cortside.Common.Messages.Filters;
using Cortside.Common.Messages.Results;
using Cortside.Health.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

        public static IServiceCollection AddApiControllers(this IServiceCollection services) {
            services.AddControllers(options => {
                options.CacheProfiles.Add("Default", new CacheProfile {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
                options.Filters.Add<MessageExceptionResponseFilter>();
                options.Conventions.Add(new ApiControllerVersionConvention());
            })
                .ConfigureApiBehaviorOptions(options => {
                    options.InvalidModelStateResponseFactory = context => {
                        var result = new ValidationFailedResult(context.ModelState);
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        return result;
                    };
                })
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;

                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    options.SerializerSettings.Converters.Add(new IsoTimeSpanConverter());
                })
                .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(HealthController).Assembly));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
            });

            return services;
        }

        public static IServiceCollection AddApiDefaults(this IServiceCollection services) {
            // add response compression using gzip and brotli compression
            services.AddDefaultResponseCompression(CompressionLevel.Optimal);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddOptions();
            services.AddApiControllers();

            // warm all the serivces up, can chain these together if needed
            services.AddStartupTask<WarmupServicesStartupTask>();

            return services;
        }
    }
}
