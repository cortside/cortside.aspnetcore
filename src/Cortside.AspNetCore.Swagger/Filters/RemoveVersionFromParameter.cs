using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cortside.AspNetCore.Swagger.Filters {
    public class RemoveVersionFromParameter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            if (operation.Parameters.Count == 0) {
                return;
            }

            var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
            if (versionParameter != null) {
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}
