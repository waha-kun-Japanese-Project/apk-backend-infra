using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public class Notification: BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Message { get; set; }
        public NotificationType Type { get; set; }
        public bool Read { get; set; }

        public Guid UserId { get; set; }
        public Guid? RelatedIssueId { get; set; }
        public Issue? RelatedIssue { get; set; }

    }
}
