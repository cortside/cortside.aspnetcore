using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    public class NoOpTransaction : IDbContextTransaction {
        public Guid TransactionId { get; } = Guid.NewGuid();

        public void Commit() {
            // Method intentionally left empty.
        }

        public Task CommitAsync(CancellationToken cancellationToken = default) {
            // Method intentionally left empty.
            return Task.CompletedTask;
        }


        // https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync


        public void Dispose() {
            // Method intentionally left empty.
        }

        public ValueTask DisposeAsync() {
            // Method intentionally left empty.
            return ValueTask.CompletedTask;
        }

        public void Rollback() {
            // Method intentionally left empty.
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default) {
            // Method intentionally left empty.
            return Task.CompletedTask;
        }
    }
}
