using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Issue.Service.Concurrency
{
    public sealed  class ExpertAssignmentGate
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<T> RunExclusiveAsync<T>(Func<Task<T>> criticalSection, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await criticalSection();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
