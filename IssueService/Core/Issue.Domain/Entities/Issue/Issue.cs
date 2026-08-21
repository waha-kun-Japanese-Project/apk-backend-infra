using Issue.Domain.Entities.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Issue.Domain.Entities.Issue
{
    public  class Issue:BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty; // e.g. "leak", "distribution_problem"
        public IssueStatus Status { get; set; } = IssueStatus.Reported;
        public IssuePriority Priority { get; set; } = IssuePriority.Medium;
        public Guid ReporterId { get; set; } // user who reported the issue
        public Guid? AssignedExpertId { get; set; } // expert assigned to the issue
        public RepairSchedule? RepairSchedule { get; set; }
        public IssueFeedback? Feedback { get; set; }

        public Guid ReportId { get; set; }

        public ICollection<ExpertReviews> ExpertReviews { get; set; } = new List<ExpertReviews>();
        public ICollection<ResolutionActions> ResolutionActions { get; set; } = new List<ResolutionActions>();
        public ICollection<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
