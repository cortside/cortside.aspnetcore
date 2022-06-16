using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cortside.AspNetCore.EntityFramework {
    public interface IUnitOfWork : IDisposable {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
        Task<IDbContextTransaction> BeginReadUncommitedAsync();
        IDbContextTransaction BeginNoTracking();
        IExecutionStrategy CreateExecutionStrategy();
    }
}
