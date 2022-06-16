using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    /// <summary>
    /// A psuedo transaction that will wrap a transaction and change the change tracking behavior back to what it was at start.
    /// NOTE: this is not thread safe, other uses of the context will get the tracking behavior for the duration of this class.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction" />
    public class NoTrackingTransaction : IDbContextTransaction {
        private readonly IDbContextTransaction innerTx;
        private readonly ChangeTracker tracker;
        private readonly QueryTrackingBehavior initialQueryTrackingBehavior;

        public NoTrackingTransaction(ChangeTracker tracker, IDbContextTransaction tx) {
            innerTx = tx;
            this.initialQueryTrackingBehavior = tracker.QueryTrackingBehavior;
            tracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.tracker = tracker;
        }

        public Guid TransactionId => innerTx.TransactionId;

        public void Commit() {
            innerTx.Commit();
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default) {
            await innerTx.CommitAsync(cancellationToken);
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }

        public void Dispose() {
            innerTx.Dispose();
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }

        public async ValueTask DisposeAsync() {
            await innerTx.DisposeAsync();
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }

        public void Rollback() {
            innerTx.Rollback();
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default) {
            await innerTx.RollbackAsync(cancellationToken);
            tracker.QueryTrackingBehavior = initialQueryTrackingBehavior;
        }
    }
}
