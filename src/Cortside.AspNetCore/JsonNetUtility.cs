using Cortside.AspNetCore.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cortside.AspNetCore {
    public static class JsonNetUtility {
        public static JsonSerializerSettings GlobalDefaultSettings() {
            return ApplyGlobalDefaultSettings(new JsonSerializerSettings(), InternalDateTimeHandling.Utc);
        }

        public static JsonSerializerSettings GlobalDefaultSettings(InternalDateTimeHandling internalDateTimeHandling) {
            return ApplyGlobalDefaultSettings(new JsonSerializerSettings(), internalDateTimeHandling);
        }

        public static JsonSerializerSettings ApplyGlobalDefaultSettings(JsonSerializerSettings settings, InternalDateTimeHandling internalDateTimeHandling) {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.DefaultValueHandling = DefaultValueHandling.Include;

            // datetime specific handling
            // always output ISO-8601 format
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            // parse using DateTimeOffset so that ISO-8601 with timezone is used
            settings.DateParseHandling = DateParseHandling.DateTimeOffset;

            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

            // intentionally commented out because of conflict with
            // Microsoft.AspNetCore.Mvc.Testing > 6.0.7 and Microsoft.NET.Test.Sdk > 17.2.0
            // https://github.com/Microsoft/testfx/issues/566
            //settings.Converters.Add(new IsoTimeSpanConverter());

            // setting to control how DateTime and DateTimeOffset are serialized.
            // always serialize to utc
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return settings;
        }
    }
}
