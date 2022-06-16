using System.Threading.Tasks;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    public class UnitOfWorkContext : AuditableDatabaseContext, IUnitOfWork {
        public UnitOfWorkContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal) : base(options, subjectPrincipal) {
        }

        public Task<IDbContextTransaction> BeginReadUncommitedAsync() {
            return Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
        }

        public IDbContextTransaction BeginNoTracking() {
            return new NoTrackingTransaction(ChangeTracker, new NoOpTransaction());
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel) {
            var tx = await Database.BeginTransactionAsync(isolationLevel);

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
