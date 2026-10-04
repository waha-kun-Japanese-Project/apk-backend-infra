using AutoMapper;
using Hangfire;
using MassTransit;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Service.Specifications;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;


namespace Report.Service.BackgroundJop
{
    public class IssueCreationJob(IUnitOfWork unitOfWork,IMapper mapper, IBackgroundJobClient  backgroundJobClient,
       IPublishEndpoint publishEndpoint) : IIssueCreationJob
    {
        public async Task CreateIssueAsync(CreateIssueRequest createIssueRequest)
        {
            var issue = mapper.Map<Issue>(createIssueRequest);
            
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
