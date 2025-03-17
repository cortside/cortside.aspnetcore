using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cortside.AspNetCore {
    public class WarmupServicesStartupTask : IStartupTask {
        private readonly IServiceProvider provider;
        private readonly ILogger<WarmupServicesStartupTask> logger;

        public WarmupServicesStartupTask(ILogger<WarmupServicesStartupTask> logger, IServiceProvider provider) {
            this.logger = logger;
            this.provider = provider;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) {
            var timer = new Stopwatch();
            timer.Start();
            logger.LogInformation("starting warmup task");

            var types = new List<Type>();
            types.AddRange(GetSingletons(provider.GetRequiredService<IServiceCollection>()));
            types.AddRange(GetControllers(provider.GetRequiredService<IServiceCollection>()));

            foreach (var singleton in types) {
                try {
                    provider.GetServices(singleton);
                } catch (Exception ex) {
                    logger.LogError(ex, $"Unable to resolve type {singleton.FullName} during warmup");
                }
            }

            timer.Stop();
            logger.LogInformation("warmup took {elapsedMilliseconds} ms", timer.ElapsedMilliseconds);
            return Task.CompletedTask;
        }

        static List<Type> GetSingletons(IServiceCollection services) {
            return services
                .Where(descriptor =>
                    descriptor.Lifetime == ServiceLifetime.Singleton &&
                    descriptor.ImplementationType != typeof(WarmupServicesStartupTask) &&
                    !descriptor.ServiceType.ContainsGenericParameters)
                .Select(descriptor => descriptor.ServiceType)
                .Distinct()
                .ToList();
        }

        static List<Type> GetControllers(IServiceCollection services) {
            return services
                .Where(descriptor =>
                    typeof(ControllerBase).IsAssignableFrom(descriptor.ImplementationType) &&
                    !descriptor.ImplementationType.IsAbstract)
                .Select(descriptor => descriptor.ServiceType)
                .Distinct()
                .ToList();
        }
    }
}
