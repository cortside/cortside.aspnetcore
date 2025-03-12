using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.EntityFramework.Tests.Contexts {
    public class UtcContext<TSubject> : AuditableDatabaseContext<TSubject> where TSubject : Subject {
        public UtcContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(options, subjectPrincipal, subjectFactory) {
            // do nothing, should be same as setting:
            // DateTimeHandling = InternalDateTimeHandling.Utc
        }

        public DbSet<Person> People { get; set; }
    }
}
