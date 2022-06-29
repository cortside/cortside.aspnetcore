using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    public class UnitOfWorkContext<TSubject> : AuditableDatabaseContext<TSubject>, IUnitOfWork where TSubject : Subject {
        public UnitOfWorkContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<TSubject> subjectFactory) : base(options, subjectPrincipal, subjectFactory) {
        }

        public Task<IDbContextTransaction> BeginReadUncommitedAsync() {
            return Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
        }

        public IDbContextTransaction BeginNoTracking() {
            return new NoTrackingTransaction(ChangeTracker, new NoOpTransaction());
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel) {
            var tx = await Database.BeginTransactionAsync(isolationLevel).ConfigureAwait(false);

            // default to no tracking when reading uncommitted with assumption/expectation that data will be used in read only fashion
            if (isolationLevel == System.Data.IsolationLevel.ReadUncommitted) {
                return new NoTrackingTransaction(ChangeTracker, tx);
            }

            return tx;
        }

        public IExecutionStrategy CreateExecutionStrategy() {
            return Database.CreateExecutionStrategy();
        }
    }
}
