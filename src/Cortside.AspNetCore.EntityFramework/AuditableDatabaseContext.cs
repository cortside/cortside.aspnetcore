using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;

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
    }
}
