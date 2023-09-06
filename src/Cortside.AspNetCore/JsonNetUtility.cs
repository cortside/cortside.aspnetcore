using Cortside.Common.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cortside.AspNetCore {
    public static class JsonNetUtility {
        public static JsonSerializerSettings GlobalDefaultSettings() {
            return ApplyGlobalDefaultSettings(new JsonSerializerSettings());
        }

        public static JsonSerializerSettings ApplyGlobalDefaultSettings(JsonSerializerSettings settings) {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.DefaultValueHandling = DefaultValueHandling.Include;

            // datetime specific handling
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.DateParseHandling = DateParseHandling.DateTimeOffset;

            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            settings.Converters.Add(new IsoTimeSpanConverter());

            return settings;
        }
    }
}
