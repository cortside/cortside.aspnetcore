using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.ModelBinding {
    /// <summary>
    /// Model binder to convert UTC to local in query param iso8601 compliant formatted dates
    /// </summary>
    public class UtcDateTimeModelBinder : IModelBinder {
        public static readonly Type[] SUPPORTED_TYPES = new Type[] { typeof(DateTime), typeof(DateTime?) };
        private readonly InternalDateTimeHandling internalDateTimeHandling;

        public UtcDateTimeModelBinder(InternalDateTimeHandling internalDateTimeHandling) {
            this.internalDateTimeHandling = internalDateTimeHandling;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext) {
            if (bindingContext == null) {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (!SUPPORTED_TYPES.Contains(bindingContext.ModelType)) {
                return Task.CompletedTask;
            }

            var modelName = GetModelName(bindingContext);
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None) {
                return Task.CompletedTask;
            }

            var modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            var metadata = bindingContext.ModelMetadata;

            var dateToParse = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(dateToParse)) {
                return Task.CompletedTask;
            }

            try {
                var dateTime = DateTime.Parse(dateToParse, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);
                if (internalDateTimeHandling == InternalDateTimeHandling.Utc) {
                    dateTime = dateTime.ToUniversalTime();
                }

                bindingContext.Result = ModelBindingResult.Success(dateTime);
            } catch (Exception ex) {
                // Conversion failed.
                modelState.TryAddModelError(modelName, ex, metadata);
            }

            return Task.CompletedTask;
        }

        private string GetModelName(ModelBindingContext bindingContext) {
            // The "Name" property of the ModelBinder attribute can be used to specify the
            // route parameter name when the action parameter name is different from the route parameter name.
            // For instance, when the route is /api/{birthDate} and the action parameter name is "date".
            // We can add this attribute with a Name property [DateTimeModelBinder(Name ="birthDate")]
            // Now bindingContext.BinderModelName will be "birthDate" and bindingContext.ModelName will be "date"

            return !string.IsNullOrEmpty(bindingContext.BinderModelName)
                ? bindingContext.BinderModelName
                : bindingContext.ModelName;
        }
    }

    public class DateTimeModelBinderAttribute : ModelBinderAttribute {
        public string DateFormat { get; set; }

        public DateTimeModelBinderAttribute() : base(typeof(UtcDateTimeModelBinder)) {
        }
    }
}
