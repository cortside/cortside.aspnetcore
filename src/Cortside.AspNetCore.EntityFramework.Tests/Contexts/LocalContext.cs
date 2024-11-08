using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.Common;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.EntityFramework.Tests.Contexts {
    public class LocalContext<TSubject> : AuditableDatabaseContext<TSubject> where TSubject : Subject {
        public LocalContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(options, subjectPrincipal, subjectFactory) {
            DateTimeHandling = InternalDateTimeHandling.Local;
        }

        public DbSet<Person> People { get; set; }
    }
}
