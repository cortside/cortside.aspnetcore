using System;
using System.Threading.Tasks;
using Cortside.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.AspNetCore.Tests.Controllers {
    [Route("api/[controller]")]
    public class EchoController : Controller {
        [Route("echo-date/{date}")]
        [HttpGet]
        public async Task<IActionResult> EchoDate(DateTime? date) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(date);
        }

        [Route("echo-custom-date/{date}")]
        [HttpGet]
        public async Task<IActionResult> EchoCustomDateFormat(
            [DateTimeModelBinder(DateFormat = "yyyyMMdd")]
            DateTime? date) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(date);
        }

        [Route("echo-model")]
        [HttpPost]
        public async Task<IActionResult> EchoModel([FromBody] PostData model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(model);
        }
    }
}
