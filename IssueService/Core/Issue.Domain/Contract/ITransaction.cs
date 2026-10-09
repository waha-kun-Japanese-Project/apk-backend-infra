using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Contract
{
    public interface ITransaction
    {
        Task<IDbContextTransaction> BeginTransactionAsync(
       IsolationLevel isolationLevel,
       CancellationToken cancellationToken = default);

        // SQL Server distributed application lock
        Task AcquireLockAsync(
            string resource,
            int timeoutMilliseconds,
            CancellationToken cancellationToken = default);
    }
}
