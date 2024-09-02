using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cortside.AspNetCore.Common;
using Cortside.AspNetCore.Tests.Controllers;
using Cortside.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace Cortside.AspNetCore.Tests {
    public class IntegrationTest {
        private TestServer server;
        private readonly Dictionary<string, string> configurationValues;

        public IntegrationTest() {
            configurationValues = new Dictionary<string, string> {
                {"Key1", "Value1"},
                {"Nested:Key1", "NestedValue1"},
                {"Nested:Key2", "NestedValue2"}
            };
        }

        private TestServer CreateTestServer(InternalDateTimeHandling internalDateTimeHandling) {
            JsonConvert.DefaultSettings = () => JsonNetUtility.GlobalDefaultSettings(internalDateTimeHandling);

            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration(config => {
                    config.AddInMemoryCollection(configurationValues);
                })
                .ConfigureServices(services => {
                    services.AddApiDefaults(internalDateTimeHandling);
                })
                .Configure(app => {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapControllers());
                });

            return new TestServer(builder);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage) {
            HttpResponseMessage responseMessage;
            using (new ScopedLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"))) {
                responseMessage = await server.CreateClient().SendAsync(requestMessage);
            }

            return responseMessage;
        }

        private async Task<HttpResponseMessage> PostAsync(string uri, string json) {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage;
            responseMessage = await server.CreateClient().PostAsync(uri, content);

            return responseMessage;

        }

        [Theory]
        [InlineData(InternalDateTimeHandling.Utc, "2000-10-02 12:00:00 AM", "2000-10-02T00:00:00Z")]
        [InlineData(InternalDateTimeHandling.Utc, "2000-10-02T00:00:00-5:00", "2000-10-02T05:00:00Z")]
        [InlineData(InternalDateTimeHandling.Local, "2000-10-02 12:00:00 AM", "2000-10-02T00:00:00Z")]
        [InlineData(InternalDateTimeHandling.Local, "2000-10-02T00:00:00-5:00", "2000-10-02T05:00:00Z")]
        public async Task ShouldGet(InternalDateTimeHandling internalDateTimeHandling, string value, string expected) {
            // Arrange
            server = CreateTestServer(internalDateTimeHandling);
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"/api/echo/echo-date/{value}");

            // Act
            var responseMessage = await SendAsync(requestMessage);

            // Assert
            var content = await responseMessage.Content.ReadAsStringAsync();
            content = content.Replace("\"", "");
            content.Should().Be(expected);
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("Mountain Standard Time", InternalDateTimeHandling.Utc, "2000-10-02 12:00:00 AM", "2000-10-02T00:00:00.0000000Z")]
        [InlineData("Mountain Standard Time", InternalDateTimeHandling.Utc, "2000-10-02T00:00:00-5:00", "2000-10-02T05:00:00.0000000Z")]
        [InlineData("Mountain Standard Time", InternalDateTimeHandling.Local, "2000-10-02 12:00:00 AM", "2000-10-02T00:00:00.0000000Z")]
        [InlineData("Mountain Standard Time", InternalDateTimeHandling.Local, "2000-10-02T00:00:00-5:00", "2000-10-02T05:00:00.0000000Z")]
        public async Task ShouldPost(string timezone, InternalDateTimeHandling internalDateTimeHandling, string value, string expected) {
            // Arrange
            server = CreateTestServer(internalDateTimeHandling);
            var json = "{\"DateFrom\":\"" + value + "\",\"DateTo\":\"2000-10-03\"}";

            using (new ScopedLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timezone))) {
                // Act
                var responseMessage = await PostAsync("/api/echo/echo-model", json);

                // Assert
                var content = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PostData>(content, new JsonSerializerSettings());
                data.DateFrom.Kind.Should().Be(DateTimeKind.Utc);
                data.DateFrom.ToString("O").Should().Be(expected);
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
