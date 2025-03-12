using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cortside.AspNetCore.Tests.Controllers {
    public class UtcDateTimeBaseController : Controller {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public UtcDateTimeBaseController(IServiceProvider services) {
            IOptions<MvcNewtonsoftJsonOptions> newtonsoftOptions =
                services.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
            jsonSerializerSettings = newtonsoftOptions.Value.SerializerSettings.CloneJsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
        }

        public override JsonResult Json(object data) {
            return Json(data, jsonSerializerSettings);
        }
    }
}
