using System.Linq;
using Cortside.Common.Messages.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.Filters.Results {
    public class ValidationFailedResult : ModelStateResult {
        public ValidationFailedResult(ModelStateDictionary modelState) : base(modelState) {
            var errors = modelState.Keys
                        .SelectMany(key => modelState[key].Errors.Select(x => new ErrorModel(x.Exception?.GetType()?.Name ?? "ModelStateValidation", key, x.ErrorMessage)))
                        .ToList();
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
