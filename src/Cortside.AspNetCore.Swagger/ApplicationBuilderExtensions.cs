using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Cortside.AspNetCore.Swagger {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, string apiName, IApiVersionDescriptionProvider provider) {
            builder.UseSwagger();
            builder.UseSwaggerUI(options => {
                options.RoutePrefix = "swagger";
                options.ShowExtensions();
                options.ShowCommonExtensions();
                options.EnableValidator();
                options.EnableFilter();
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);

                foreach (var description in provider.ApiVersionDescriptions) {
                    var version = description.GroupName.ToLowerInvariant();
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{apiName} " + version.ToUpper());
                }
            });

            foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName)) {
                builder.UseReDoc(c => {
                    var version = groupName.ToLowerInvariant();
                    c.DocumentTitle = $"{apiName} Documentation {groupName.ToUpperInvariant()}";
                    c.RoutePrefix = $"api-docs/{version}";
                    c.SpecUrl = $"/swagger/{version}/swagger.json";
                });
            }

            return builder;
        }
    }
}
