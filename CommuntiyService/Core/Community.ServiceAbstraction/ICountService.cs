using Communtiy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.ServiceAbstraction
{
    public interface ICountService
    {
        Task<long> GetVoteCountAsync(Guid issueId);
        Task<long> GetShareCountAsync(Guid issueId);
        Task<long> GetCommentCountAsync(Guid issueId);
        Task<long> IncrementVoteAsync(Guid issueId);
        Task<long> DecrementVoteAsync(Guid issueId);
        Task<long> IncrementShareAsync(Guid issueId);
        Task<long> IncreamentCommentAsync(Guid issueId);
        Task<long> DecrementCommentAsync(Guid issueId);
    }
}
