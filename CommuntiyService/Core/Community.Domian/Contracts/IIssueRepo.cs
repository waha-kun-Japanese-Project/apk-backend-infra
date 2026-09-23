using Community.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Contracts;

public interface IIssueRepo
{
    Task<Issue?> GetByIdAsync(
            Guid issueId,
            CancellationToken cancellationToken = default);

    Task<IEnumerable<Issue>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
