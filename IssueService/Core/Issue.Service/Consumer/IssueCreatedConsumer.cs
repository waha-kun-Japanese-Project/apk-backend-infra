using CommanLib.EventNotification.IssueEvent;
using Hangfire;
using Issue.ServiceAbstraction.Jop;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Consumer
{
    public class IssueCreatedConsumer(IBackgroundJobClient backgroundJobClient) : IConsumer<IssueCreatedEvent>
    {
        public Task Consume(ConsumeContext<IssueCreatedEvent> context)
        {
            backgroundJobClient.Enqueue<IExpertAssignmentJob>(
            job => job.ExecuteAsync(context.Message.issueId, CancellationToken.None));

            return Task.CompletedTask;
        }
    }
}
