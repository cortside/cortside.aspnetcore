using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Asp.Versioning;
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
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration,
            string title, string description, string[] versions) {
            var apiversions = new List<OpenApiInfo>();
            foreach (var version in versions) {
                apiversions.Add(new OpenApiInfo {
                    Version = version,
                    Title = title,
                    Description = description
                });
            }

            var xmlFile = $"{Assembly.GetCallingAssembly()?.GetName().Name}.xml";
            services.AddSwagger(configuration, xmlFile, apiversions);

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration,
            string xmlFile, List<OpenApiInfo> versions) {
            services.AddApiVersioning(
                    options => {
                        // reporting api versions will return the headers
                        // "api-supported-versions" and "api-deprecated-versions"
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = false;
                        //options.UseApiBehavior = true;

                        options.Policies.Sunset(0.9)
                            .Effective(DateTimeOffset.Now.AddDays(60))
                            .Link("policy.html")
                            .Title("Versioning Policy")
                            .Type("text/html");
                    })
                .AddMvc()
                .AddApiExplorer(
                    options => {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;

                        options.AssumeDefaultVersionWhenUnspecified = false;
                    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c => {
                foreach (var version in versions) {
                    c.SwaggerDoc(version.Version, version);
                }

                // Set the comments path for the Swagger JSON and UI.               
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //c.CustomSchemaIds(type => type.ToString());

                // custom operationIds
                c.CustomSchemaIds(x => x.FullName);
                c.CustomOperationIds(e => {
                    var controller = e.ActionDescriptor.RouteValues["controller"];
                    var action = e.ActionDescriptor.RouteValues["action"];

                    // remove implementation detail from operationId
                    if (action.EndsWith("Async")) {
                        action = action.Substring(0, action.Length - 5);
                    }

                    return $"{controller}_{action}";
                });

                c.OperationFilter<RemoveVersionFromParameter>();
                c.OperationFilter<AuthorizeOperationFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                //c.IgnoreObsoleteActions(); -- don't use this so can use Obsolete attribute in controller methods to communicate with other teams
                //c.TagActionsBy(apiDescription => new[] { apiDescription.RelativePath });

                var identityServerConfiguration =
                    configuration.GetSection("IdentityServer").Get<IdentityServerConfiguration>();
                var authority = identityServerConfiguration?.Authority;

                if (!string.IsNullOrWhiteSpace(authority)) {
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows {
                            ClientCredentials = new OpenApiOAuthFlow {
                                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                                TokenUrl = new Uri($"{authority}/connect/token"),
                                Scopes = new Dictionary<string, string> {
                                    { identityServerConfiguration.ApiName, identityServerConfiguration.ApiName }
                                }
                            }
                        }
                    });
                }
            });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
