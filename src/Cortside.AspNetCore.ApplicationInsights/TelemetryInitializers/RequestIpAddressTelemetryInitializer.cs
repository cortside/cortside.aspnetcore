using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class RequestIpAddressTelemetryInitializer : ITelemetryInitializer {
        private const string PROPERTY_KEY = "RequestIpAddress";
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestIpAddressTelemetryInitializer(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry) {
            if (telemetry is not RequestTelemetry requestTelemetry) {
                return;
            }

            if (!requestTelemetry.Properties.ContainsKey(PROPERTY_KEY)) {
                requestTelemetry.Properties.Add(PROPERTY_KEY, HttpContextUtility.GetRequestIpAddress(httpContextAccessor.HttpContext));
            }
        }
    }
}
