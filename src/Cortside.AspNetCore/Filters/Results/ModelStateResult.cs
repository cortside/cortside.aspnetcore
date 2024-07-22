using System;
using System.Linq;
using Cortside.Common.Messages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.Filters.Results {
    public class ModelStateResult : ObjectResult {
        public ModelStateResult(ModelStateDictionary modelState) : base(new ErrorsModel(modelState.Keys
                        .SelectMany(key => modelState[key].Errors.Select(x => new ErrorModel(x.Exception?.GetType()?.Name ?? "ModelStateValidation", key, x.ErrorMessage)))
                        .ToList())) { }
        //public ModelStateResult(ModelStateDictionary modelState) : base("some string") { }
    }
}
