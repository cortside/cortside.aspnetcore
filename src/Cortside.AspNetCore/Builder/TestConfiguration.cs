using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Cortside.AspNetCore.Builder {
    /// <summary>
    /// https://github.com/dotnet/aspnetcore/issues/37680#issuecomment-1331559463
    /// </summary>
    public static class TestConfiguration {
        // This async local is set in from tests and it flows to main
        internal static readonly AsyncLocal<Action<IConfigurationBuilder>> current = new();

        /// <summary>
        /// Adds the current test configuration to the application in the "right" place
        /// </summary>
        /// <param name="configurationBuilder">The configuration builder</param>
        /// <returns>The modified <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddTestConfiguration(this IConfigurationBuilder configurationBuilder) {
            if (current.Value is { } configure) {
                configure(configurationBuilder);
            }

            return configurationBuilder;
        }

        /// <summary>
        /// Unit tests can use this to flow state to the main program and change configuration
        /// </summary>
        /// <param name="action"></param>
        public static void Create(Action<IConfigurationBuilder> action) {
            current.Value = action;
        }
    }
}
