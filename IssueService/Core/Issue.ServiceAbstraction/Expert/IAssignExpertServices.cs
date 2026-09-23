using Issue.Shared.DTOS.AssignExpert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.ServiceAbstraction.Expert
{
    public  interface IAssignExpertServices
    {
        Task<AssignExpertResponse> AssignExpertAsync(Guid issueId, AssignExpertRequest request, CancellationToken cancellationToken = default);

        Task<AssignExpertResponse> AutoAssignExpertAsync(Guid issueId, CancellationToken cancellationToken = default);

        Task<AssignExpertResponse> UpdateAssignedExpertAsync(Guid issueId, AssignExpertRequest request, CancellationToken cancellationToken = default);

        Task UnassignExpertAsync(Guid issueId, CancellationToken cancellationToken = default);


    }
}
