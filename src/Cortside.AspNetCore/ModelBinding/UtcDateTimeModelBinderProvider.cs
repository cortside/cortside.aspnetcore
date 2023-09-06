using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.ModelBinding {
    /// <summary>
    /// Converts UTC datetimes in query params to local time
    /// Should remove this if product-api is ever fully converted to run in utc timezone in kubernetes cluster
    /// </summary>
    public class UtcDateTimeModelBinderProvider : IModelBinderProvider {
        private readonly DateTimeHandling dateTimeHandling;

        public UtcDateTimeModelBinderProvider(DateTimeHandling dateTimeHandling) {
            this.dateTimeHandling = dateTimeHandling;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (UtcDateTimeModelBinder.SUPPORTED_TYPES.Contains(context.Metadata.ModelType)) {
                if (dateTimeHandling == DateTimeHandling.Utc) {
                    return new UtcDateTimeModelBinder(DateTimeStyles.AssumeUniversal);
                } else {
                    return new UtcDateTimeModelBinder(DateTimeStyles.AssumeLocal);
                }
            }

            return null;
        }
    }
}
