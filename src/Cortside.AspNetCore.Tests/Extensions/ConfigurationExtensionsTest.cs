using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace Cortside.AspNetCore.Tests.Extensions {
    public class ConfigurationExtensionsTest {
        [Fact]
        public void ShouldExpandTemplates() {
            // arrange
            var inMemoryConfigSettings = new Dictionary<string, string>() {
                { "Database:ConnectionString", "Server={{Database:Host}};Initial Catalog={{Database:Name}};" },
                { "Database:Host", "localhost" },
                { "Database:Name", "testing" },
                { "RootLevel", "{{Database:Name}}" }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfigSettings)
                .Build();

            // act
            config.ExpandTemplates();

            // assert
            config["Database:ConnectionString"].ShouldBe("Server=localhost;Initial Catalog=testing;");
            config["RootLevel"].ShouldBe("testing");
        }

        [Fact]
        public void ShouldExpandTemplatesInArrays() {
            // arrange
            var inMemoryConfigSettings = new Dictionary<string, string>() {
                { "ASPNETCORE_ENVIRONMENT", "Dev" },
                { "MailFeature:0:Subject", "{{ASPNETCORE_ENVIRONMENT}} Subject Marketing" },
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfigSettings)
                .Build();

            // act
            config.ExpandTemplates();

            // assert
            config["MailFeature:0:Subject"].ShouldBe("Dev Subject Marketing");
        }

        [Fact]
        public void ShouldExpandTemplatesButIgnoreUnmatched() {
            // arrange
            var inMemoryConfigSettings = new Dictionary<string, string>() {
                { "TemplateHasNoMatchingKey", "{{Blah}} {{Blerg}}" },
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfigSettings)
                .Build();

            // act
            config.ExpandTemplates();

            // assert
            config["TemplateHasNoMatchingKey"].ShouldBe("{{Blah}} {{Blerg}}");
        }
    }
}
