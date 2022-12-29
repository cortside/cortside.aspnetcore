using Serilog.Core;
using Serilog.Events;

namespace Cortside.AspNetCore.Enrichers {
    public class OperationIdEnricher : ILogEventEnricher {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
            if (logEvent.Properties.TryGetValue("RequestId", out var requestId)) {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("operationId", requestId));
            }
        }
    }
}
