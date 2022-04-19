using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.Filters {
    public static class MvcBuilderExtensions {
        public static IMvcBuilder AddApiControllerVersion(this IMvcBuilder builder) {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Services.Configure<MvcOptions>(options => options.Conventions.Add(new ApiControllerVersionConvention()));
            return builder;
        }
    }
}
