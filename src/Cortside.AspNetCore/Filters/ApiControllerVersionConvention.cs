using System.Collections.Generic;
using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Cortside.AspNetCore.Filters {
    public class ApiControllerVersionConvention : IControllerModelConvention {
        public void Apply(ControllerModel controller) {
            if (!(controller.ControllerType.IsDefined(typeof(ApiVersionAttribute)) ||
                  controller.ControllerType.IsDefined(typeof(ApiVersionNeutralAttribute)))) {
                if (controller.Attributes is List<object>
                    attributes) {
                    attributes.Add(new ApiVersionNeutralAttribute());
                }
            }
        }
    }
}
