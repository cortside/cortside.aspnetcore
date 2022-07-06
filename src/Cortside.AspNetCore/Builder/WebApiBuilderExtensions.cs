using System;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.Builder {
    public static class WebApiBuilderExtensions {
        public static WebApiBuilder ConfigureServices(this WebApiBuilder builder, Action<IServiceCollection> action) {
            action(builder.Services);
            return builder;
        }
    }
}
