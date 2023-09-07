// https://www.vickram.me/custom-datetime-model-binding-in-asp-net-core-web-api

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


            /*
                        var modelState = bindingContext.ModelState;
                        modelState.SetModelValue(modelName, valueProviderResult);

                        var metadata = bindingContext.ModelMetadata;
                        var type = metadata.UnderlyingOrModelType;
                        try
                        {
                            var value = valueProviderResult.FirstValue;

                            object? model;
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                // Parse() method trims the value (with common DateTimeSyles) then throws if the result is empty.
                                model = null;
                            }
                            else if (type == typeof(DateTime))
                            {
                                model = DateTime.Parse(value, valueProviderResult.Culture, _supportedStyles);
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }

                            // When converting value, a null model may indicate a failed conversion for an otherwise required
                            // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
                            // current bindingContext. If not, an error is logged.
                            if (model == null && !metadata.IsReferenceOrNullableType)
                            {
                                modelState.TryAddModelError(
                                    modelName,
                                    metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                                        valueProviderResult.ToString()));
                            }
                            else
                            {
                                bindingContext.Result = ModelBindingResult.Success(model);
                            }
                        }
                        catch (Exception exception)
                        {
                            // Conversion failed.
                            modelState.TryAddModelError(modelName, exception, metadata);
                        }
            */


            var dateToParse = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(dateToParse)) {
                return Task.CompletedTask;
            }

            try {
                //if (DateTime.TryParse(dateToParse, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime validDate)) {
                //    return validDate;
                //}

                //var dateTime = Helper.ParseDateTime(dateToParse);
                var dateTime = DateTime.Parse(dateToParse, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal); //dateTimeStyle);
                if (internalDateTimeHandling == InternalDateTimeHandling.Utc) {
                    dateTime = dateTime.ToUniversalTime();
                }
                //if (dateTimeStyle == DateTimeStyles.AssumeUniversal) {
                //    dateTime = dateTime.ToUniversalTime();
                //}

                bindingContext.Result = ModelBindingResult.Success(dateTime);

                //if (dateTime != null) {
                //    var modelState = bindingContext.ModelState;
                //    modelState.SetModelValue(modelName, valueProviderResult);

                //    var metadata = bindingContext.ModelMetadata;
                //    var type = metadata.UnderlyingOrModelType;

                //    // Conversion failed.
                //    modelState.TryAddModelError(modelName, exception, metadata);
                //} else {
                //    bindingContext.Result = ModelBindingResult.Success(dateTime);
                //}
            } catch (Exception ex) {
                // Conversion failed.
                modelState.TryAddModelError(modelName, ex, metadata);
            }


            return Task.CompletedTask;
        }

        //private DateTime? ParseDate(ModelBindingContext bindingContext, string dateToParse) {
        //    var attribute = GetDateTimeModelBinderAttribute(bindingContext);
        //    var dateFormat = attribute?.DateFormat;

        //    if (string.IsNullOrEmpty(dateFormat)) {
        //        return Helper.ParseDateTime(dateToParse);
        //    }

        //    return Helper.ParseDateTime(dateToParse, new string[] { dateFormat });
        //}

        //private DateTimeModelBinderAttribute GetDateTimeModelBinderAttribute(ModelBindingContext bindingContext) {
        //    var modelName = GetModelName(bindingContext);

        //    var paramDescriptor = bindingContext.ActionContext.ActionDescriptor.Parameters
        //        .Where(x => x.ParameterType == typeof(DateTime?))
        //        .Where((x) => {
        //            // See comment in GetModelName() on why we do this.
        //            var paramModelName = x.BindingInfo?.BinderModelName ?? x.Name;
        //            return paramModelName.Equals(modelName);
        //        })
        //        .FirstOrDefault();

        //    var ctrlParamDescriptor = paramDescriptor as ControllerParameterDescriptor;
        //    if (ctrlParamDescriptor == null) {
        //        return null;
        //    }

        //    var attribute = ctrlParamDescriptor.ParameterInfo
        //        .GetCustomAttributes(typeof(DateTimeModelBinderAttribute), false)
        //        .FirstOrDefault();

        //    return (DateTimeModelBinderAttribute)attribute;
        //}

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

        //private readonly DateTimeStyles _supportedStyles;

        ///// <summary>
        ///// constructor
        ///// </summary>
        ///// <param name="supportedStyles"></param>
        ///// <exception cref="ArgumentNullException"></exception>
        //public UtcDateTimeModelBinder(DateTimeStyles supportedStyles) {
        //    _supportedStyles = supportedStyles;
        //}

        ///// <summary>
        ///// Bind model
        ///// </summary>
        ///// <param name="bindingContext"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="NotSupportedException"></exception>
        //public Task BindModelAsync(ModelBindingContext bindingContext) {
        //    if (bindingContext == null) {
        //        throw new ArgumentNullException(nameof(bindingContext));
        //    }

        //    var modelName = bindingContext.ModelName;
        //    var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        //    if (valueProviderResult == ValueProviderResult.None) {
        //        // no entry
        //        return Task.CompletedTask;
        //    }

        //    var modelState = bindingContext.ModelState;
        //    modelState.SetModelValue(modelName, valueProviderResult);

        //    var metadata = bindingContext.ModelMetadata;
        //    var type = metadata.UnderlyingOrModelType;

        //    var value = valueProviderResult.FirstValue;
        //    var culture = valueProviderResult.Culture;

        //    object model;
        //    if (string.IsNullOrWhiteSpace(value)) {
        //        model = null;
        //    } else if (type == typeof(DateTime)) {
        //        // You could put custom logic here to sniff the raw value and call other DateTime.Parse overloads, use TryParse instead etc
        //        model = DateTime.Parse(value, culture).ToLocalTime();
        //    } else {
        //        // unreachable
        //        throw new NotSupportedException();
        //    }

        //    // When converting value, a null model may indicate a failed conversion for an otherwise required
        //    // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
        //    // current bindingContext. If not, an error is logged.
        //    if (model == null && !metadata.IsReferenceOrNullableType) {
        //        modelState.TryAddModelError(
        //            modelName,
        //            metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
        //                valueProviderResult.ToString()));
        //    } else {
        //        bindingContext.Result = ModelBindingResult.Success(model);
        //    }

        //    return Task.CompletedTask;
    }

    public class DateTimeModelBinderAttribute : ModelBinderAttribute {
        public string DateFormat { get; set; }

        public DateTimeModelBinderAttribute() : base(typeof(UtcDateTimeModelBinder)) {
        }
    }

    //public class Helper {
    //    public static DateTime? ParseDateTime(string dateToParse, string[] formats = null, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None) {
    //        var CUSTOM_DATE_FORMATS = new string[] {
    //            "yyyyMMddTHHmmssZ",
    //            "yyyyMMddTHHmmZ",
    //            "yyyyMMddTHHmmss",
    //            "yyyyMMddTHHmm",
    //            "yyyyMMddHHmmss",
    //            "yyyyMMddHHmm",
    //            "yyyyMMdd",
    //            "yyyy-MM-ddTHH-mm-ss",
    //            "yyyy-MM-dd-HH-mm-ss",
    //            "yyyy-MM-dd-HH-mm",
    //            "yyyy-MM-dd",
    //            "MM-dd-yyyy"
    //        };

    //        if (formats == null || !formats.Any()) {
    //            formats = CUSTOM_DATE_FORMATS;
    //        }

    //        if (DateTime.TryParse(dateToParse, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime validDate)) {
    //            return validDate;
    //        }

    //        //foreach (var format in formats) {
    //        //    if (format.EndsWith("Z")) {
    //        //        if (DateTime.TryParseExact(dateToParse, format, provider, DateTimeStyles.AssumeUniversal, out validDate)) {
    //        //            return validDate;
    //        //        }
    //        //    }

    //        //    if (DateTime.TryParseExact(dateToParse, format, provider, styles, out validDate)) {
    //        //        return validDate;
    //        //    }
    //        //}

    //        return null;
    //    }

    //    public static bool IsNullableType(Type type) {
    //        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    //    }
    //}
}
