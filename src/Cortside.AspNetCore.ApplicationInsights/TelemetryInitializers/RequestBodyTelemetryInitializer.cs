using System.IO;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class RequestBodyTelemetryInitializer : ITelemetryInitializer {
        private const string PROPERTY_KEY = "RequestBody";
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestBodyTelemetryInitializer(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry) {
            if (telemetry is RequestTelemetry requestTelemetry) {
                if ((httpContextAccessor.HttpContext.Request.Method == HttpMethods.Post ||
                        httpContextAccessor.HttpContext.Request.Method == HttpMethods.Put) &&
                        httpContextAccessor.HttpContext.Request.Body.CanRead) {

                    if (requestTelemetry.Properties.ContainsKey(PROPERTY_KEY)) {
                        return;
                    }

                    httpContextAccessor.HttpContext.Request.EnableRewind();
                    var sr = new StreamReader(httpContextAccessor.HttpContext.Request.Body);
                    var bodyContent = sr.ReadToEnd();
                    httpContextAccessor.HttpContext.Request.Body.Position = 0;
                    requestTelemetry.Properties.Add(PROPERTY_KEY, bodyContent);
                }
            }
        }
    }
}
