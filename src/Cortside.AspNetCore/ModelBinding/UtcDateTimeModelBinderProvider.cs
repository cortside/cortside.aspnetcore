using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cortside.AspNetCore.ModelBinding {
    /// <summary>
    /// Converts DateTIme values in models to utc or local time based on InternalDateTimeHandling
    /// </summary>
    public class UtcDateTimeModelBinderProvider : IModelBinderProvider {
        private readonly InternalDateTimeHandling internalDateTimeHandling;

        public UtcDateTimeModelBinderProvider(InternalDateTimeHandling internalDateTimeHandling) {
            this.internalDateTimeHandling = internalDateTimeHandling;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (UtcDateTimeModelBinder.SUPPORTED_TYPES.Contains(context.Metadata.ModelType)) {
                return new UtcDateTimeModelBinder(internalDateTimeHandling);
            }

            return null;
        }
    }
}
