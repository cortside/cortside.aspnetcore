using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.EntityFramework.Conventions;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cortside.AspNetCore.EntityFramework {
    public class AuditableDatabaseContext<TSubject> : DbContext where TSubject : Subject {
        private readonly ISubjectPrincipal subjectPrincipal;
        private readonly ISubjectFactory<TSubject> subjectFactory;

        public AuditableDatabaseContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(options) {
            this.subjectPrincipal = subjectPrincipal;
            this.subjectFactory = subjectFactory;
        }

        public DbSet<TSubject> Subjects { get; set; }

        public async Task<int> SaveChangesAsync() {
            await SetAuditableEntityValuesAsync();
            return await base.SaveChangesAsync(default).ConfigureAwait(false);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            await SetAuditableEntityValuesAsync();
            return await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
            await SetAuditableEntityValuesAsync();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Use SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int SaveChanges() {
            throw new NotImplementedException("intentionally not implemented, use Async methods");
        }

        private async Task SetAuditableEntityValuesAsync() {
            // check for subject in subjects set and either create or get to attach to AudibleEntity
            var updatingSubject = await GetCurrentSubjectAsync();
            ChangeTracker.DetectChanges();
            var modified = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity && (x.State == EntityState.Modified || x.State == EntityState.Added));
            var added = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity && x.State == EntityState.Added);

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
            foreach (var item in modified) {
                ((AuditableEntity)item.Entity).LastModifiedSubject = updatingSubject;
                ((AuditableEntity)item.Entity).LastModifiedDate = DateTime.Now.ToUniversalTime();
            }

            foreach (var item in added) {
                ((AuditableEntity)item.Entity).CreatedSubject = updatingSubject;
                ((AuditableEntity)item.Entity).CreatedDate = DateTime.Now.ToUniversalTime();
            }
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions

            await OnBeforeSaveChangesAsync(updatingSubject);
        }

        /// <summary>
        /// override this method to put in custom handling before changes are actually saved
        /// </summary>
        /// <remarks>
        /// This method can be used to save way that SaveChangesAsync could have been
        /// </remarks>
        /// <returns></returns>
        protected virtual Task OnBeforeSaveChangesAsync(Subject updatingSubject) {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets or creates the current subject record.
        /// </summary>
        /// <returns></returns>
        private async Task<Subject> GetCurrentSubjectAsync() {
            var subjectId = Guid.Parse(subjectPrincipal.SubjectId);

            var subject = Subjects.Local.FirstOrDefault(s => s.SubjectId == subjectId);
            subject ??= await Subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId).ConfigureAwait(false);

            if (subject != null) {
                return subject;
            }

            // create new subject if one is not found
            subject = subjectFactory.CreateSubject(subjectPrincipal);
            Subjects.Add(subject);
            return subject;
        }

        protected static void SetDateTime(ModelBuilder builder) {
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

        protected static void SetCascadeDelete(ModelBuilder builder) {
            var fks = builder.Model.GetEntityTypes().SelectMany(t => t.GetDeclaredForeignKeys());
            foreach (var fk in fks) {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }

#if (NET8_0_OR_GREATER)
        /// <summary>
        /// This adds a convention to apply HasTriggers to all tables in the model
        /// </summary>
        /// <remarks>
        /// Cortside applies triggers to all tables by default, which require mitigation in EF7+
        /// <see href="https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes?tabs=v7#mitigations-2"></see>
        /// </remarks>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
#endif

    }
}
