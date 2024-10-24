using System;
using System.Linq;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.Common;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cortside.AspNetCore.EntityFramework.Interceptors {
    public abstract class AuditableSaveChangesInterceptor<TSubject> : SaveChangesInterceptor where TSubject : Subject {
        protected readonly InternalDateTimeHandling dateTimeHandling;
        protected readonly ISubjectPrincipal subjectPrincipal;
        protected readonly ISubjectFactory<TSubject> subjectFactory;
        protected readonly DbSet<TSubject> subjects;

        protected AuditableSaveChangesInterceptor(DbSet<TSubject> subjects, InternalDateTimeHandling dateTimeHandling, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) {
            this.subjects = subjects;
            this.dateTimeHandling = dateTimeHandling;
            this.subjectPrincipal = subjectPrincipal;
            this.subjectFactory = subjectFactory;
        }

        /// <summary>
        /// Gets or creates the current subject record.
        /// </summary>
        /// <returns></returns>
        protected async Task<TSubject> GetCurrentSubjectAsync() {
            var subjectId = Guid.Parse(subjectPrincipal.SubjectId);

            var subject = subjects.Local.FirstOrDefault(s => s.SubjectId == subjectId);
            subject ??= await subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId).ConfigureAwait(false);

            if (subject != null) {
                return subject;
            }

            // create new subject if one is not found
            subject = subjectFactory.CreateSubject(subjectPrincipal);
            subjects.Add(subject);
            return subject;
        }
    }
}
