using System;
using System.Threading.Tasks;
using Cortside.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.AspNetCore.Tests.Controllers {
    [Route("api/[controller]")]
    public class EchoController : UtcDateTimeBaseController {
        public EchoController(IServiceProvider services) : base(services) { }

        [Route("echo-date/{date}")]
        [HttpGet]
        public Task<IActionResult> EchoDate(DateTime? date) {
            if (!ModelState.IsValid) {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            return Task.FromResult<IActionResult>(Ok(date));
        }

        [Route("echo-custom-date/{date}")]
        [HttpGet]
        public Task<IActionResult> EchoCustomDateFormat(
            [DateTimeModelBinder(DateFormat = "yyyyMMdd")]
            DateTime? date) {
            if (!ModelState.IsValid) {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            return Task.FromResult<IActionResult>(Ok(date));
        }

        [Route("echo-model")]
        [HttpPost]
        public Task<IActionResult> EchoModel([FromBody] PostData model) {
            if (!ModelState.IsValid) {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            return Task.FromResult<IActionResult>(Ok(model));
        }
    }
}
