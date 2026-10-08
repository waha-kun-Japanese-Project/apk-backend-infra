using Issue.Domain.Contract;
using Issue.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Repository
{
    public class Transaction(IssueDbContext dbContext) : ITransaction
    {
        public async Task AcquireLockAsync(string resource, int timeoutMilliseconds, CancellationToken cancellationToken = default)
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
            EXEC sp_getapplock
                @Resource = {resource},
                @LockMode = 'Exclusive',
                @LockOwner = 'Transaction',
                @LockTimeout = {timeoutMilliseconds}
            """,
            cancellationToken);

        }

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
           return dbContext.Database.BeginTransactionAsync(isolationLevel,
            cancellationToken);
        }
    }
}
