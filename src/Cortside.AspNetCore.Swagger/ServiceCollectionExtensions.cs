using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration) {
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
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "Acme.ShoppingCart API",
                    Description = "Acme.ShoppingCart API",
                });

                c.SwaggerDoc("v2", new OpenApiInfo {
                    Version = "v2",
                    Title = "Acme.ShoppingCart API",
                    Description = "Acme.ShoppingCart API",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<RemoveVersionFromParameter>();
                c.OperationFilter<AuthorizeOperationFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                c.IgnoreObsoleteActions();
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