#pragma warning disable CS0618 // use of obsolete method/property

using Newtonsoft.Json;

namespace Cortside.AspNetCore.Tests.Controllers {
    public static class JsonSerializerSettingsExtensions {
        public static JsonSerializerSettings CloneJsonSerializerSettings(this JsonSerializerSettings settings) {
            var cloneSettings = new JsonSerializerSettings {
                StringEscapeHandling = settings.StringEscapeHandling,
                FloatParseHandling = settings.FloatParseHandling,
                FloatFormatHandling = settings.FloatFormatHandling,
                DateParseHandling = settings.DateParseHandling,
                DateTimeZoneHandling = settings.DateTimeZoneHandling,
                DateFormatHandling = settings.DateFormatHandling,
                Formatting = settings.Formatting,
                MaxDepth = settings.MaxDepth,
                DateFormatString = settings.DateFormatString,
                Context = settings.Context,
                Error = settings.Error,
                Binder = settings.Binder,
                SerializationBinder = settings.SerializationBinder,
                TraceWriter = settings.TraceWriter,
                Culture = settings.Culture,
                ReferenceResolver = settings.ReferenceResolver,
                ReferenceResolverProvider = settings.ReferenceResolverProvider,
                EqualityComparer = settings.EqualityComparer,
                ContractResolver = settings.ContractResolver,
                ConstructorHandling = settings.ConstructorHandling,
                TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat,
                TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling,
                MetadataPropertyHandling = settings.MetadataPropertyHandling,
                TypeNameHandling = settings.TypeNameHandling,
                PreserveReferencesHandling = settings.PreserveReferencesHandling,
                Converters = settings.Converters,
                DefaultValueHandling = settings.DefaultValueHandling,
                NullValueHandling = settings.NullValueHandling,
                ObjectCreationHandling = settings.ObjectCreationHandling,
                MissingMemberHandling = settings.MissingMemberHandling,
                ReferenceLoopHandling = settings.ReferenceLoopHandling,
                CheckAdditionalContent = settings.CheckAdditionalContent
            };

            return cloneSettings;
        }
    }
}
