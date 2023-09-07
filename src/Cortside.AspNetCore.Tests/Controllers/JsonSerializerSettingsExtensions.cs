using Newtonsoft.Json;

namespace Cortside.AspNetCore.Tests.Controllers {
    public static class JsonSerializerSettingsExtensions {
        public static JsonSerializerSettings CloneJsonSerializerSettings(this JsonSerializerSettings settings) {
            JsonSerializerSettings cloneSettings = new JsonSerializerSettings();
            cloneSettings.StringEscapeHandling = settings.StringEscapeHandling;
            cloneSettings.FloatParseHandling = settings.FloatParseHandling;
            cloneSettings.FloatFormatHandling = settings.FloatFormatHandling;
            cloneSettings.DateParseHandling = settings.DateParseHandling;
            cloneSettings.DateTimeZoneHandling = settings.DateTimeZoneHandling;
            cloneSettings.DateFormatHandling = settings.DateFormatHandling;
            cloneSettings.Formatting = settings.Formatting;
            cloneSettings.MaxDepth = settings.MaxDepth;
            cloneSettings.DateFormatString = settings.DateFormatString;
            cloneSettings.Context = settings.Context;
            cloneSettings.Error = settings.Error;
            cloneSettings.SerializationBinder = settings.SerializationBinder;
            cloneSettings.Binder = settings.Binder;
            cloneSettings.TraceWriter = settings.TraceWriter;
            cloneSettings.Culture = settings.Culture;
            cloneSettings.ReferenceResolverProvider = settings.ReferenceResolverProvider;
            cloneSettings.EqualityComparer = settings.EqualityComparer;
            cloneSettings.ContractResolver = settings.ContractResolver;
            cloneSettings.ConstructorHandling = settings.ConstructorHandling;
            cloneSettings.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
            cloneSettings.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
            cloneSettings.MetadataPropertyHandling = settings.MetadataPropertyHandling;
            cloneSettings.TypeNameHandling = settings.TypeNameHandling;
            cloneSettings.PreserveReferencesHandling = settings.PreserveReferencesHandling;
            cloneSettings.Converters = settings.Converters;
            cloneSettings.DefaultValueHandling = settings.DefaultValueHandling;
            cloneSettings.NullValueHandling = settings.NullValueHandling;
            cloneSettings.ObjectCreationHandling = settings.ObjectCreationHandling;
            cloneSettings.MissingMemberHandling = settings.MissingMemberHandling;
            cloneSettings.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            cloneSettings.ReferenceResolver = settings.ReferenceResolver;
            cloneSettings.CheckAdditionalContent = settings.CheckAdditionalContent;

            return cloneSettings;
        }
    }
}
