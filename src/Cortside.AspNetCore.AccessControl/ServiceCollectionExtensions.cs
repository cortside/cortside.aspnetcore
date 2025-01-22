using System;
#if (NET8_0_OR_GREATER)
using Cortside.Authorization.Client.AspNetCore;
#endif
using Cortside.Common.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (NET8_0_OR_GREATER)
using Microsoft.IdentityModel.JsonWebTokens;
#endif

namespace Cortside.AspNetCore.AccessControl {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Adds the access control using IdentityServer and PolicyServer. Sections named IdentityServer and PolicyServer
        /// are expected to be found in configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddAccessControl(this IServiceCollection services, IConfiguration configuration) {
            Guard.From.Null(configuration, nameof(configuration));
            Guard.Against(() => !configuration.GetSection("IdentityServer").Exists(), () => throw new ArgumentException("Configuration section named 'IdentityServer' is missing"));

#if (NET8_0_OR_GREATER)
            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
#else
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
#endif

            var identityServerConfiguration = configuration.GetSection("IdentityServer").Get<IdentityServerConfiguration>();
            Guard.From.NullOrWhitespace(identityServerConfiguration.Authority, nameof(identityServerConfiguration.Authority), "IdentityServer:Authority is null");
            Guard.From.NullOrWhitespace(identityServerConfiguration.Authentication?.ClientId, nameof(identityServerConfiguration.Authentication.ClientId), "IdentityServer:Authentication:ClientId is null");
            Guard.From.NullOrWhitespace(identityServerConfiguration.Authentication?.ClientSecret, nameof(identityServerConfiguration.Authentication.ClientSecret), "IdentityServer:Authentication:ClientSecret is null");

            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //.AddOpenIdConnect(o => o.mapinboundclaims=false)
                .AddIdentityServerAuthentication(options => {
                    // base-address of your identityserver
                    options.Authority = identityServerConfiguration.Authority;
                    options.RequireHttpsMetadata = identityServerConfiguration.RequireHttpsMetadata;
                    options.RoleClaimType = "role";
                    options.NameClaimType = "name";

                    // name of the API resource
                    options.ApiName = identityServerConfiguration.ApiName;
                    options.ApiSecret = identityServerConfiguration.ApiSecret;

                    options.EnableCaching = identityServerConfiguration.EnableCaching;
                    options.CacheDuration = identityServerConfiguration.CacheDuration;
                });

            // authorization provider
            var accessControlConfiguration = configuration.GetSection("AccessControl").Exists() ? configuration.GetSection("AccessControl").Get<AccessControlConfiguration>() : new AccessControlConfiguration();

            // policy server
            if (accessControlConfiguration.AuthorizationProvider == AccessControlProviders.PolicyServer) {
                Guard.Against(() => !configuration.GetSection("PolicyServer").Exists(), () => throw new ArgumentException("Configuration section named 'PolicyServer' is missing"));
                configuration["PolicyServer:TokenClient:Authority"] = identityServerConfiguration.Authority;
                configuration["PolicyServer:TokenClient:ClientId"] = identityServerConfiguration.Authentication?.ClientId;
                configuration["PolicyServer:TokenClient:ClientSecret"] = identityServerConfiguration.Authentication?.ClientSecret;
                services.AddPolicyServerRuntimeClient(configuration.GetSection("PolicyServer"))
                    .AddAuthorizationPermissionPolicies();
            }

            // authorization-api
            if (accessControlConfiguration.AuthorizationProvider == AccessControlProviders.AuthorizationApi) {
#if (NET8_0_OR_GREATER)
                services.AddAuthorizationApiClient(configuration);
#else
                throw new ArgumentException($"{AccessControlProviders.AuthorizationApi} requires net8 or greater", nameof(accessControlConfiguration.AuthorizationProvider));
#endif
            }

            return services;
        }
    }
}
