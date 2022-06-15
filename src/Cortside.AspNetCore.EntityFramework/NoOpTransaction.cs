using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    public class NoOpTransaction : IDbContextTransaction {
        public Guid TransactionId => throw new NotImplementedException();

        public void Commit() {
            // Method intentionally left empty.
        }

        public Task CommitAsync(CancellationToken cancellationToken = default) {
            // Method intentionally left empty.
            return Task.CompletedTask;
        }

        public void Dispose() {
            // Method intentionally left empty.
        }

        public ValueTask DisposeAsync() {
            // Method intentionally left empty.
            return ValueTask.CompletedTask;
        }

        public void Rollback() {
            throw new NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }
    }
}
