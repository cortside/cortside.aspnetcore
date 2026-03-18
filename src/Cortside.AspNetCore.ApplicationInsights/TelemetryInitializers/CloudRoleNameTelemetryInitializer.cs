using System.Collections.Generic;
using OpenTelemetry.Resources;

namespace Cortside.AspNetCore.ApplicationInsights.TelemetryInitializers {
    public class CloudRoleNameTelemetryInitializer : IResourceDetector {
        private const string SERVICE_NAME = "service.name";
        private readonly string cloudRoleName;

        public CloudRoleNameTelemetryInitializer(string cloudRoleName) {
            this.cloudRoleName = cloudRoleName;
        }

        public Resource Detect() {
            return new Resource(new Dictionary<string, object> {
                [SERVICE_NAME] = cloudRoleName
            });
        }
    }
}
