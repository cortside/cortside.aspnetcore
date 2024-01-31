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

        /// <summary>
        /// Use BeginReadUncommittedAsync on GET endpoints that return a list, this will read uncommitted and
        /// as notracking in ef core.  this will result in a non-blocking dirty read, which is accepted best practice for mssql
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginReadUncommitedAsync() {
            return Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// Using BeginNoTracking on GET endpoints for a single entity so that data is read committed
        /// with assumption that it might be used for changes in future calls
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginNoTracking() {
            return new NoTrackingTransaction(ChangeTracker, new NoOpTransaction());
        }

        /// <summary>
        /// Start a database transaction.  If the isolationLevel is ReadUncommitted, it will disable change tracker (AsNoTracking).
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
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
