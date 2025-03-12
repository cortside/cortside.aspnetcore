using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cortside.AspNetCore.Middleware {
    public class RequestIpAddressLoggingMiddleware {
        private const string PROPERTY_KEY = "RequestIpAddress";
        private readonly ILogger<RequestIpAddressLoggingMiddleware> logger;
        private readonly RequestDelegate next;

        public RequestIpAddressLoggingMiddleware(ILogger<RequestIpAddressLoggingMiddleware> logger, RequestDelegate next) {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var properties = new Dictionary<string, object> {
                [PROPERTY_KEY] = HttpContextUtility.GetRequestIpAddress(context)
            };

            using (logger.BeginScope(properties)) {
                await next(context);
            }
        }
    }
}
