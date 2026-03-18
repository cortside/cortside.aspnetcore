using System;
using Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers;
using Cortside.Common.Validation;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Cortside.AspNetCore.ApplicationInsights {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Add TelemetryInitializer to set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <returns></returns>
        public static IServiceCollection AddCloudRoleNameInitializer(this IServiceCollection services,
            string cloudRoleName) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));
            var resourceDetector = new CloudRoleNameTelemetryInitializer(cloudRoleName);

            services.AddHttpContextAccessor();

            services.ConfigureOpenTelemetryTracerProvider((sp, tracerBuilder) => {
                tracerBuilder.ConfigureResource(resource => resource.AddDetector(resourceDetector));
                tracerBuilder.AddProcessor(new RequestIpAddressTelemetryInitializer(sp.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>()));
            });

            services.ConfigureOpenTelemetryLoggerProvider((_, loggerBuilder) => {
                loggerBuilder.ConfigureResource(resource => resource.AddDetector(resourceDetector));
            });

            services.ConfigureOpenTelemetryMeterProvider((_, meterBuilder) => {
                meterBuilder.ConfigureResource(resource => resource.AddDetector(resourceDetector));
            });

            return services;
        }

        /// <summary>
        /// Add ApplicationInsights telemetry and set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <param name="instrumentationKey"></param>
        /// <returns></returns>
        [Obsolete("Use of InstrumentationKey has been obsoleted, use override with ApplicationInsightsServiceOptions")]
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, string cloudRoleName,
            string instrumentationKey) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));
            Guard.From.NullOrWhitespace(instrumentationKey, nameof(instrumentationKey));
            // TODO: add logging stating missing connection string or instrumentation key

            services.AddApplicationInsightsTelemetry(o => {
                o.ConnectionString = $"InstrumentationKey={instrumentationKey}";
                o.SamplingRatio = 1.0f;
            });

            services.AddCloudRoleNameInitializer(cloudRoleName);
            return services;
        }

        /// <summary>
        /// Add ApplicationInsights telemetry and set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, string cloudRoleName,
            ApplicationInsightsServiceOptions options) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));
            Guard.From.Null(options, nameof(options));
            // TODO: add logging stating missing connection string or instrumentation key

            services.AddApplicationInsightsTelemetry(o => {
                o.ConnectionString = options.ConnectionString;
                o.SamplingRatio = 1.0f;
            });
            services.AddCloudRoleNameInitializer(cloudRoleName);
            return services;
        }
    }
}
