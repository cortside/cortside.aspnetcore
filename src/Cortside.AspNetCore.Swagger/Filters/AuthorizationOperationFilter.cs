using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cortside.AspNetCore.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cortside.AspNetCore.Swagger.Filters {
    public class AuthorizeOperationFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            var isPost = context.GetControllerAndActionAttributes<HttpPostAttribute>().Any();
            var isPut = context.GetControllerAndActionAttributes<HttpPutAttribute>().Any();

            // TODO: should probably be in another filter or this one renamed
            var code400 = ((int)HttpStatusCode.BadRequest).ToString();
            if (!operation.Responses.ContainsKey(code400) && (isPost || isPut)) {
                AddError(operation, code400, context);
            }

            var authorizationAttributes = context.GetControllerAndActionAttributes<AuthorizeAttribute>();
            if (!authorizationAttributes.Any()) {
                return;
            }

            operation.Parameters ??= new List<OpenApiParameter>();

            // display input parameter Authorization
            operation.Parameters.Add(new OpenApiParameter {
                Name = "Authorization",
                @In = ParameterLocation.Header,
                Description = "access token",
                Required = false,
                Schema = new OpenApiSchema {
                    Type = "string"
                }
            });

            // display possible response status codes
            var code401 = ((int)HttpStatusCode.Unauthorized).ToString();
            if (!operation.Responses.ContainsKey(code401)) {
                operation.Responses.Add(code401, new OpenApiResponse { Description = "Unauthorized" });
            }

            var authorizeAttribute = authorizationAttributes.OfType<AuthorizeAttribute>().FirstOrDefault();
            if (authorizeAttribute != null && !string.IsNullOrWhiteSpace(authorizeAttribute.Policy)) {
                var code403 = ((int)HttpStatusCode.Forbidden).ToString();
                if (!operation.Responses.ContainsKey(code403)) {
                    operation.Responses.Add(code403, new OpenApiResponse { Description = "Forbidden" });
                }
            }

            // display required authentication/policy
            var authorizationDescription = " (Auth)";
            if (authorizeAttribute != null && !string.IsNullOrWhiteSpace(authorizeAttribute.Policy)) {
                authorizationDescription = $" (Auth permission: {authorizeAttribute.Policy})";
            }

            operation.Summary ??= "";
            operation.Summary += authorizationDescription;

            operation.Security.Add(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme() {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme, Id = "oauth2"
                        }
                    },
                    new List<string>()
                }
            });
        }

        private static void AddError(OpenApiOperation operation, string statusString, OperationFilterContext context) {
            if (!operation.Responses.TryGetValue(statusString, out var response)) {
                response = new OpenApiResponse();
                operation.Responses.Add(statusString, response);
            }

            response.Description = "BadRequest";
            response.Content ??= new Dictionary<string, OpenApiMediaType>();

            var openApiMediaType = new OpenApiMediaType();

            var type = typeof(ErrorsModel);
            if (!context.SchemaRepository.TryLookupByType(type, out var schema)) {
                schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
                if (schema == null) {
                    throw new InvalidOperationException($"Failed to register swagger schema type '{type.Name}'");
                }
            }

            // TODO: look for produces attribute
            openApiMediaType.Schema = schema;
            response.Content.Add("application/json", openApiMediaType);
        }
    }
}
