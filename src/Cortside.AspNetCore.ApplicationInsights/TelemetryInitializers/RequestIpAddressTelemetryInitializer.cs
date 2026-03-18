using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class RequestIpAddressTelemetryInitializer : BaseProcessor<Activity> {
        private const string PROPERTY_KEY = "RequestIpAddress";
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestIpAddressTelemetryInitializer(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override void OnEnd(Activity data) {
            if (data.Kind != ActivityKind.Server) {
                return;
            }

            if (data.GetTagItem(PROPERTY_KEY) is null) {
                data.SetTag(PROPERTY_KEY, HttpContextUtility.GetRequestIpAddress(httpContextAccessor.HttpContext));
            }
        }
    }
}
