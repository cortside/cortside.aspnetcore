﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cortside.AspNetCore.Middleware;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cortside.AspNetCore.Hosting {
    /// <summary>
    /// Base timed hosted service
    /// </summary>
    public abstract class TimedHostedService : BackgroundService {
        protected readonly ILogger logger;
        private readonly int interval;
        private readonly bool enabled;
        private readonly bool generateCorrelationId;

        /// <summary>
        /// Initializes new instance of the Hosted Service
        /// </summary>
        protected TimedHostedService(ILogger logger, bool enabled, int interval, bool generateCorrelationId = true) {
            this.logger = logger;
            this.interval = interval;
            this.enabled = enabled;
            this.generateCorrelationId = generateCorrelationId;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            // force async so that hosted service does not block Startup
            await Task.Yield();

            if (enabled) {
                logger.LogInformation($"{GetType().Name} is starting with interval of {interval} seconds");

                stoppingToken.Register(() => logger.LogDebug($"{GetType().Name} is stopping."));

                while (!stoppingToken.IsCancellationRequested) {
                    await IntervalAsync();
                    await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken).ConfigureAwait(false);
                }
                logger.LogInformation($"{GetType().Name} is stopping");
            } else {
                logger.LogInformation($"{GetType().Name} is disabled");
            }
        }

        private async Task IntervalAsync() {
            var correlationId = CorrelationContext.GetCorrelationId(generateCorrelationId);
            using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId })) {
                logger.LogDebug($"{GetType().Name} is working");

                try {
                    await ExecuteIntervalAsync().ConfigureAwait(false);
                } catch (Exception ex) {
                    logger.LogError(ex, GetType().Name);
                }
            }
        }

        protected abstract Task ExecuteIntervalAsync();
    }
}
