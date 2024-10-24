using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.Common;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cortside.AspNetCore.EntityFramework.Interceptors {
    public class AuditableInterceptor<TSubject> : AuditableSaveChangesInterceptor<TSubject> where TSubject : Subject {

        public AuditableInterceptor(DbSet<TSubject> subjects, InternalDateTimeHandling dateTimeHandling, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(subjects, dateTimeHandling, subjectPrincipal, subjectFactory) {
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default) {
            // check for subject in subjects set and either create or get to attach to AudibleEntity
            var updatingSubject = await GetCurrentSubjectAsync();

            var now = dateTimeHandling == InternalDateTimeHandling.Utc ? DateTime.UtcNow : DateTime.Now;

            DbContext? dbContext = eventData.Context;
            if (dbContext is null) {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var entities = dbContext.ChangeTracker.Entries<AuditableEntity>().Where(e => e is { State: EntityState.Added or EntityState.Modified });

            foreach (var entityEntry in entities) {
                entityEntry.Entity.LastModifiedSubject = updatingSubject;
                entityEntry.Entity.LastModifiedDate = now;

                if (entityEntry.State == EntityState.Added) {
                    entityEntry.Entity.CreatedSubject = updatingSubject;
                    entityEntry.Entity.CreatedDate = now;
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
