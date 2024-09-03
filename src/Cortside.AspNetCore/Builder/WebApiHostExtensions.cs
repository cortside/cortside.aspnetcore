using System;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.Builder {
    public static class WebApiHostExtensions {
        public static WebApiHost Configure(this WebApiHost api, Action<IApplicationBuilder, IWebHostEnvironment, IApiVersionDescriptionProvider> action) {
            var provider = api.Services.GetService<IApiVersionDescriptionProvider>();
            action(api.WebApplication, api.WebApplication.Environment, provider);
            return api;
        }
    }
}
