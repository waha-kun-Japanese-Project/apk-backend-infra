using Hangfire;
using Issue.Domain.Contract;
using Issue.Service.Specifications.ExpertSpecifications;
using Issue.ServiceAbstraction.Jop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Jop
{
    public class ExpertAssignmentReconciliationJob(
    IUnitOfWork unitOfWork,
    IBackgroundJobClient backgroundJobClient)
    {
        public async Task ExecuteAsync()
        {
            var repository =
                unitOfWork.GetRepository<
                    Issue.Domain.Entities.Issue.Issue,
                    Guid>();

            var issues =
                await repository.GetAllAsync(
                    new UnassignedDiagnosedIssuesSpecification());

            foreach (var issue in issues)
            {
                backgroundJobClient.Enqueue<IExpertAssignmentJob>(
                    job => job.ExecuteAsync(
                        issue.Id,
                        CancellationToken.None));
            }
        }



    }
}