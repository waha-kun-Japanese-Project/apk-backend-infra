using AutoMapper;
using Hangfire;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Service.Specifications;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.BackgroundJop
{
    public class IssueCreationJob(IUnitOfWork unitOfWork,IMapper mapper, IBackgroundJobClient  backgroundJobClient) : IIssueCreationJob
    {
        public async Task CreateIssueAsync(CreateIssueRequest createIssueRequest)
        {
            var issue = mapper.Map<Issue>(createIssueRequest);

            // Create the required GPSLocation for this issue
            var gpsLocation = new GPSLocation
            {
                Latitude = createIssueRequest.Latitude ?? string.Empty,
                Longitude = createIssueRequest.Longitude ?? string.Empty
            };

            unitOfWork
                .GetRepository<GPSLocation, Guid>()
                .Add(gpsLocation);

            issue.GPSLocationId = gpsLocation.Id;
            issue.GPSLocation = gpsLocation;

            unitOfWork
                .GetRepository<Issue, Guid>()
                .Add(issue);

            var attachmentrepo =  unitOfWork
                .GetRepository<IssueAttachment, Guid>();
            var attachment= await attachmentrepo.GetByIdAsync(new EntityByIdSpecification<IssueAttachment>(createIssueRequest.IssueAttachmentId));

            attachment.IssueId = issue.Id;
         attachmentrepo.Update(attachment);
           await unitOfWork.SaveChangesAsync();
        }
    }
}
