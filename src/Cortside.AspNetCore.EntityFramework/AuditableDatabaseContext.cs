using System;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.Common;
using Cortside.AspNetCore.EntityFramework.Interceptors;
#if (NET8_0_OR_GREATER)
using Cortside.AspNetCore.EntityFramework.Conventions;
#endif
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.EntityFramework {
    public class AuditableDatabaseContext<TSubject> : DbContext where TSubject : Subject {
        protected readonly ISubjectPrincipal subjectPrincipal;
        protected readonly ISubjectFactory<TSubject> subjectFactory;

        public AuditableDatabaseContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(options) {
            this.subjectPrincipal = subjectPrincipal;
            this.subjectFactory = subjectFactory;
        }

        /// <summary>
        /// Used to control the date
        /// </summary>
        public InternalDateTimeHandling DateTimeHandling { get; set; } = InternalDateTimeHandling.Utc;

        public DbSet<TSubject> Subjects { get; set; }

        /// <summary>
        /// Use SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int SaveChanges() {
            throw new NotImplementedException("intentionally not implemented, use Async methods");
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

            base.ConfigureConventions(configurationBuilder);
        }
#endif

        /// <summary>
        /// must call base
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.AddInterceptors(new AuditableInterceptor<TSubject>(Subjects, DateTimeHandling, subjectPrincipal, subjectFactory));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
