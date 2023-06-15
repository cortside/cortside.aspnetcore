using System;
using Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers;
using Cortside.Common.Validation;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.ApplicationInsights {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Add TelemetryInitializer to set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <returns></returns>
        public static IServiceCollection AddCloudRoleNameInitializer(this IServiceCollection services, string cloudRoleName) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));

            services.AddSingleton<ITelemetryInitializer>(_ => new CloudRoleNameTelemetryInitializer(cloudRoleName));
            services.AddSingleton<ITelemetryInitializer, RequestIpAddressTelemetryInitializer>();
            return services;
        }

        /// <summary>
        /// Add ApplicationInsights telemetry and set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <param name="instrumentationKey"></param>
        /// <returns></returns>
        [Obsolete("Use of InstrumentationKey has been obsoleted, use override with ")]
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, string cloudRoleName, string instrumentationKey) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));
            // TODO: add logging stating missing connection string or instrumentation key

            services.AddApplicationInsightsTelemetry(o => {
                o.InstrumentationKey = instrumentationKey;
                o.EnableAdaptiveSampling = false;
                o.EnableActiveTelemetryConfigurationSetup = true;
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
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, string cloudRoleName, ApplicationInsightsServiceOptions options) {
            Guard.From.NullOrWhitespace(cloudRoleName, nameof(cloudRoleName));
            Guard.From.Null(options, nameof(options));
            // TODO: add logging stating missing connection string or instrumentation key

            services.AddApplicationInsightsTelemetry(o => {
                o.ConnectionString = options.ConnectionString;
                o.EnableAdaptiveSampling = false;
                o.EnableActiveTelemetryConfigurationSetup = true;
            });
            services.AddCloudRoleNameInitializer(cloudRoleName);
            return services;
        }
    }
}
