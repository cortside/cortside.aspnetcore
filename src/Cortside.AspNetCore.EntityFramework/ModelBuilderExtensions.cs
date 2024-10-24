using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cortside.AspNetCore.EntityFramework {
    public static class ModelBuilderExtensions {
        /// <summary>
        /// Use in OnModelCreating to set a ValueConverter on all DateTime/DateTime? properties for all entities
        /// </summary>
        /// <param name="builder"></param>
        public static void SetDateTime(this ModelBuilder builder) {
            // 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM
            // using local as default with the assumption that most of the time local will be utc and expected
            // OR it's not utc and that local timezone is expected to be persisted.  potential for future other configuration
            // value or use of DateTimeHandling
            var min = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var max = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Local);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
#pragma warning disable S3358 // Ternary operators should not be nested
                v => v < min ? min : v > max ? max : v,
                v => v < min ? min : v > max ? max : v);
#pragma warning restore S3358 // Ternary operators should not be nested

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
#pragma warning disable S3358 // Ternary operators should not be nested
                v => v.HasValue ? v < min ? min : v > max ? max : v : v,
                v => v.HasValue ? v < min ? min : v > max ? max : v : v);
#pragma warning restore S3358 // Ternary operators should not be nested

            foreach (var entityType in builder.Model.GetEntityTypes()) {
                foreach (var property in entityType.GetProperties()) {
                    if (property.ClrType == typeof(DateTime)) {
                        property.SetValueConverter(dateTimeConverter);
                    } else if (property.ClrType == typeof(DateTime?)) {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }

        /// <summary>
        /// Use in OnModelCreating to set DeleteBehavior to NoAction on all entity ForeignKeys
        /// </summary>
        /// <param name="builder"></param>
        public static void SetCascadeDelete(this ModelBuilder builder) {
            var fks = builder.Model.GetEntityTypes().SelectMany(t => t.GetDeclaredForeignKeys());
            foreach (var fk in fks) {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
