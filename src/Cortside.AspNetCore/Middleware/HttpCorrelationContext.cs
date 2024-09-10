using System;
using System.Linq;
using Cortside.Common.Correlation;
using Microsoft.AspNetCore.Http;

namespace Cortside.AspNetCore {
    public static class HttpCorrelationContext {
        public static string SetFromHttpContext(HttpContext context) {
            // get possible request headers for correlationId           
            var xCorrelationId = GetHeaderValue(context, "X-Correlation-Id");
            var requestId = GetHeaderValue(context, "Request-Id");

            // set with most preferred header
            string correlationId = xCorrelationId;

            // set with requestId if it's not set already
            correlationId ??= requestId;

            // set with new guid if not set
            correlationId ??= Guid.NewGuid().ToString();

            // set values
            CorrelationContext.SetCorrelationId(correlationId);
            CorrelationContext.SetRequestId(context.TraceIdentifier);
            return correlationId;
        }

        private static string GetHeaderValue(HttpContext context, string header) {
            // check for correlationId as Request-Id from request
            context.Request.Headers.TryGetValue(header, out var ids);
            var id = ids.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(id)) {
                return id;
            }

            return null;
        }
    }
}
