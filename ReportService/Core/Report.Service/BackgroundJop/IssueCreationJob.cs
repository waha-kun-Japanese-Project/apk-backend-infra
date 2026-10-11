using AutoMapper;
using CommanLib.EventNotification.IssueEvent;
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

            // Without this the job threw a NullReferenceException that only Hangfire's dashboard saw.
            if (attachment is null)
                throw new InvalidOperationException(
                    $"Attachment {createIssueRequest.IssueAttachmentId} no longer exists; issue was not created.");

            attachment.IssueId = issue.Id;
            attachmentrepo.Update(attachment);

            // The farmer's tracker (IssueService GET Farmer/{id}/tracking) is built from StatusHistory,
            // so an issue with no row here has no steps to show. Diagnosed is the status the mapper sets.
            ChangeStatus(issue, issue.Status);

           await unitOfWork.SaveChangesAsync();


              await publishEndpoint.Publish(new IssueCreatedEvent(issue.Id));
        }

        private void ChangeStatus(Issue issue, IssueStatus status)
        {
            issue.Status = status;

            unitOfWork.GetRepository<StatusHistory, Guid>().Add(new StatusHistory
            {
                IssueId = issue.Id,
                Status = status,
                ChangedById = null,
                Note = $"Issue {status} by system"
            });
        }
    }
}
