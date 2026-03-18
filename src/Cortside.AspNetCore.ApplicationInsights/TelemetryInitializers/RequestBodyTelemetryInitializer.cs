using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class RequestBodyTelemetryInitializer : BaseProcessor<Activity> {
        private const string PROPERTY_KEY = "RequestBody";
        readonly IHttpContextAccessor httpContextAccessor;

        public RequestBodyTelemetryInitializer(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override void OnEnd(Activity data) {
            var request = httpContextAccessor?.HttpContext?.Request;

            var hasReadableBody = request != null &&
                                  (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put) &&
                                  request.Body.CanRead;
            if (!hasReadableBody) {
                return;
            }

            if (data.Kind != ActivityKind.Server) {
                return;
            }

            if (data.GetTagItem(PROPERTY_KEY) is not null) {
                return;
            }

            request.EnableBuffering();
            var sr = new StreamReader(request.Body);
            var bodyContent = sr.ReadToEnd();
            request.Body.Position = 0;
            data.SetTag(PROPERTY_KEY, bodyContent);
        }
    }
}
