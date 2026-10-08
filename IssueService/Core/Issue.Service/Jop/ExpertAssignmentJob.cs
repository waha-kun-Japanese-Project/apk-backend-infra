using Hangfire;
using Issue.Service.Services;
using Issue.ServiceAbstraction.Expert;
using Issue.ServiceAbstraction.Jop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Jop
{
    public class ExpertAssignmentJob(IAssignExpertServices assignExpert) : IExpertAssignmentJob
    {
        [AutomaticRetry( Attempts = 3, DelaysInSeconds = new[] { 10, 30, 60 })]
        public async Task ExecuteAsync(Guid issueId, CancellationToken cancellationToken)
        {
            await assignExpert.AutoAssignExpertAsync(issueId,cancellationToken);

        }
    }
}