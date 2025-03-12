using System.IO;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class RequestBodyTelemetryInitializer : ITelemetryInitializer {
        private const string PROPERTY_KEY = "RequestBody";
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestBodyTelemetryInitializer(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry) {
            var request = httpContextAccessor?.HttpContext?.Request;

            var hasReadableBody = request != null && (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put) && request.Body.CanRead;
            if (!hasReadableBody) {
                return;
            }

            if (telemetry is not RequestTelemetry requestTelemetry) {
                return;
            }

            if (requestTelemetry.Properties.ContainsKey(PROPERTY_KEY)) {
                return;
            }

            request.EnableBuffering();
            var sr = new StreamReader(request.Body);
            var bodyContent = sr.ReadToEnd();
            request.Body.Position = 0;
            requestTelemetry.Properties.Add(PROPERTY_KEY, bodyContent);
        }
    }
}
