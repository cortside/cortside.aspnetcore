using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cortside.AspNetCore {
    public static class HostExtensions {
        /// <summary>
        /// Runs all of the startup tasks and then starts the host
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task RunWithTasksAsync(this WebApplication host, string url = null,
            CancellationToken cancellationToken = default) {
            // Execute all the tasks
            foreach (var startupTask in host.Services.GetServices<IStartupTask>()) {
                await startupTask.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }

            // Start the tasks as normal
            // workaround for url configuration, note that host used to be IHost -- https://github.com/dotnet/aspnetcore/issues/38185#issuecomment-963552844
            if (string.IsNullOrEmpty(url)) {
                await host.RunAsync(cancellationToken).ConfigureAwait(false);
            } else {
                await host.RunAsync(url).ConfigureAwait(false);
            }
        }
    }
}
