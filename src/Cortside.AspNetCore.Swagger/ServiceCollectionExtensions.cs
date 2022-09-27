using System;
using System.Collections.Generic;
using System.IO;
using Cortside.AspNetCore.AccessControl;
using Cortside.AspNetCore.Swagger.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Cortside.AspNetCore.Swagger {
    /// <summary>
    /// Adds swagger with versioning and OpenID Connect configuration using Newtonsoft
    /// </summary>
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, string xmlFile, List<OpenApiInfo> versions) {
            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.UseApiBehavior = true;
            });

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = false;
            });

            services.AddSwaggerGen(c => {
                foreach (var version in versions) {
                    c.SwaggerDoc(version.Version, version);
                }

                // Set the comments path for the Swagger JSON and UI.               
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<RemoveVersionFromParameter>();
                c.OperationFilter<AuthorizeOperationFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                c.TagActionsBy(c => new[] { c.RelativePath });

                var identityServerConfiguration = configuration.GetSection("IdentityServer").Get<IdentityServerConfiguration>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows {
                        ClientCredentials = new OpenApiOAuthFlow {
                            AuthorizationUrl = new Uri($"{identityServerConfiguration.Authority}/connect/authorize"),
                            TokenUrl = new Uri($"{identityServerConfiguration.Authority}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                {identityServerConfiguration.ApiName, identityServerConfiguration.ApiName}
                            }
                        }
                    }
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
