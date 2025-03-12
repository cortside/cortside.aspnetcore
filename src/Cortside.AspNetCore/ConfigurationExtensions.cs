using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Cortside.AspNetCore {
    public static class ConfigurationExtensions {

        public static IConfiguration ExpandTemplates(this IConfiguration configuration) {
            var kvps = configuration.AsEnumerable();
            foreach (var kvp in kvps) {
                var expanded = ExpandTemplate(kvp.Value, configuration);
                if (expanded != kvp.Value) {
                    configuration[kvp.Key] = expanded;
                    var sectionSegments = kvp.Key.Split(':');
                    var configurationSection = configuration.GetSection(sectionSegments[0]);
                    for (int i = 1; i < sectionSegments.Length; i++) {
                        configurationSection = configurationSection.GetSection(sectionSegments[i]);
                    }
                    configurationSection.Value = expanded;
                }
            }
            return configuration;
        }

        private static string ExpandTemplate(string template, IConfiguration config) {
            if (string.IsNullOrWhiteSpace(template)) {
                return template;
            }
            // should match each occurrence of '{{blah}}'
            Regex SubstitutionsRegex = new Regex(@"(?<=\{{)([^\{\}]+|\{[^\{\}]+\})(?=\}})", RegexOptions.Compiled);

            var keysToSubstitute = SubstitutionsRegex.Matches(template).Cast<Match>().SelectMany(m => m.Captures.Cast<Capture>()).Select(c => c.Value);
            if (keysToSubstitute.Any()) {
                foreach (var key in keysToSubstitute) {
                    var value = config[key];

                    if (value != null) {
                        template = template.Replace("{{" + key + "}}", value);
                    }
                }
                return template;
            }

            return template;
        }
    }
}
