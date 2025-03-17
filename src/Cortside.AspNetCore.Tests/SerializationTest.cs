using System;
using Cortside.Common.Json;
using Cortside.Common.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Cortside.AspNetCore.Tests {
    public class Flight {
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime DepartureDateUtc { get; set; }
        public DateTime DepartureDateLocal { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class SerializationTest {
        [Fact]
        public void Serialization() {
            var settings = JsonNetUtility.GlobalDefaultSettings();
            settings.Converters.Add(new IsoTimeSpanConverter());

            using (new ScopedLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"))) {
                Flight flight = new Flight {
                    Destination = "Dubai",
                    DepartureDate = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Unspecified),
                    DepartureDateUtc = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Utc),
                    DepartureDateLocal = new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Local),
                    Duration = TimeSpan.FromHours(5.5)
                };

                string json = JsonConvert.SerializeObject(flight, Formatting.None, settings);
                string expected =
                    """{"destination":"Dubai","departureDate":"2013-01-21T00:00:00Z","departureDateUtc":"2013-01-21T00:00:00Z","departureDateLocal":"2013-01-21T07:00:00Z","duration":"P0Y0M0DT5H30M0S"}""";
                Assert.Equal(expected, json);
            }
        }

        [Fact]
        public void Deserialization() {
            var settings = JsonNetUtility.GlobalDefaultSettings();
            settings.Converters.Add(new IsoTimeSpanConverter());

            string json =
                """{"destination":"Dubai","departureDate":"2013-01-21T00:00:00Z","departureDateUtc":"2013-01-21T00:00:00Z","departureDateLocal":"2013-01-21T07:00:00Z","duration":"P0Y0M0DT5H30M0S"}""";
            var flight = JsonConvert.DeserializeObject<Flight>(json, settings);

            Assert.Equal(DateTimeKind.Utc, flight.DepartureDate.Kind);
            Assert.Equal(DateTimeKind.Utc, flight.DepartureDateLocal.Kind);
            Assert.Equal(DateTimeKind.Utc, flight.DepartureDateUtc.Kind);
            Assert.Equal(TimeSpan.FromHours(5.5), flight.Duration);
        }
    }
}
