using System.Linq;
using Cortside.Common.Testing.Logging;
using Microsoft.Extensions.Logging;

namespace Cortside.AspNetCore.Tests {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Cortside.AspNetCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

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
