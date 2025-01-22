using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cortside.AspNetCore.AccessControl.Tests {
    public class AddAccessControlTest {
        [Fact]
        public void ShouldValidateAllConfigurationPresent() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["IdentityServer:Authentication:ClientSecret"] = "secret",
                    ["PolicyServer:BasePolicy"] = "policy"
                })
                .Build();

            services.AddAccessControl(config);
            Assert.Equal("secret", config["PolicyServer:TokenClient:ClientSecret"]);
        }

        [Fact]
        public void ShouldValidateMissingIdentityServerSection() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            var ex = Assert.Throws<ArgumentException>(() => services.AddAccessControl(config));
            Assert.Equal("Configuration section named 'IdentityServer' is missing", ex.Message);
        }

        [Fact]
        public void ShouldValidateMissingIdentityServerAuthority() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:RequireHttpsMetadata"] = "false",
                    ["PolicyServer:BasePolicy"] = "policy"
                })
                .Build();

            var ex = Assert.Throws<ArgumentException>(() => services.AddAccessControl(config));
            Assert.Equal("IdentityServer:Authority is null (Parameter 'Authority')", ex.Message);
        }

        [Fact]
        public void ShouldValidateMissingIdentityServerClientId() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:RequireHttpsMetadata"] = "false",
                    ["IdentityServer:Authority"] = "http://ids",
                    ["PolicyServer:BasePolicy"] = "policy"
                })
                .Build();

            var ex = Assert.Throws<ArgumentException>(() => services.AddAccessControl(config));
            Assert.Equal("IdentityServer:Authentication:ClientId is null (Parameter 'ClientId')", ex.Message);
        }

        [Fact]
        public void ShouldValidateMissingIdentityServerClientSecret() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:RequireHttpsMetadata"] = "false",
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["PolicyServer:BasePolicy"] = "policy"
                })
                .Build();

            var ex = Assert.Throws<ArgumentException>(() => services.AddAccessControl(config));
            Assert.Equal("IdentityServer:Authentication:ClientSecret is null (Parameter 'ClientSecret')", ex.Message);
        }

        [Fact]
        public void ShouldValidateMissingPolicyServerSection() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["IdentityServer:Authentication:ClientSecret"] = "secret",
                })
                .Build();

            var ex = Assert.Throws<ArgumentException>(() => services.AddAccessControl(config));
            Assert.Equal("Configuration section named 'PolicyServer' is missing", ex.Message);
        }

        [Fact]
        public void ShouldUseAuthorizationApiSection() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["IdentityServer:Authentication:ClientSecret"] = "secret",
                    ["AccessControl:AuthorizationProvider"] = "AuthorizationApi",
                })
                .Build();

            var exception = Record.Exception(() => services.AddAccessControl(config));
            Assert.Null(exception);
        }
    }
}
