using Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers;
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
            services.AddSingleton<ITelemetryInitializer>(_ => new CloudRoleNameTelemetryInitializer(cloudRoleName));
            return services;
        }

        /// <summary>
        /// Add ApplicationInsights telemetry and set Cloud RoleName
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cloudRoleName"></param>
        /// <param name="instrumentationKey"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, string cloudRoleName, string instrumentationKey) {
            services.AddApplicationInsightsTelemetry(o => {
                o.InstrumentationKey = instrumentationKey;
                o.EnableAdaptiveSampling = false;
                o.EnableActiveTelemetryConfigurationSetup = true;
            });
            services.AddCloudRoleNameInitializer(cloudRoleName);
            return services;
        }
    }
}
