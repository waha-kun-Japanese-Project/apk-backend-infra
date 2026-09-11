using Issue.Shared.DTOS.FarmerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.ServiceAbstraction.Farmer
{
    public interface IFarmerService
    {
        Task<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>>
            GetAllIssuesAsync(IssueFilteration issueFilteration,CancellationToken cancellationToken = default);
    }
}
