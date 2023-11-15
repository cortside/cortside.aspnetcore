using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortside.Common.Testing.Logging.LogEvent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cortside.AspNetCore.Tests {
    public class WarmupServicesStartupTaskTests {
        [Fact]
        public async Task CanCallExecuteAsync() {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IServiceCollection>(services);
            var provider = services.BuildServiceProvider();
            var logger = new LogEventLogger<WarmupServicesStartupTask>();
            var warmup = new WarmupServicesStartupTask(logger, provider);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act
            await warmup.ExecuteAsync(cts.Token);

            // Assert
            Assert.Equal(2, logger.LogEvents.Count(x => x.LogLevel == LogLevel.Information));
            Assert.Equal(0, logger.LogEvents.Count(x => x.LogLevel == LogLevel.Error));
            Assert.Equal(0, logger.LogEvents.Count(x => x.LogLevel == LogLevel.Critical));
        }
    }
}
