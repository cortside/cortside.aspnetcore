using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.Filters.Results {
    public class ValidationFailedResult : ModelStateResult {
        public ValidationFailedResult(ModelStateDictionary modelState) : base(modelState) {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
