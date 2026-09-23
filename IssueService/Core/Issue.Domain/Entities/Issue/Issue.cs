
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

        public Guid GPSLocationId { get; set; }

        public GPSLocation GPSLocation { get; set; } = null!;

        public ICollection<ExpertReviews> ExpertReviews { get; set; } = new List<ExpertReviews>();
        public ICollection<ResolutionActions> ResolutionActions { get; set; } = new List<ResolutionActions>();
        public ICollection<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<IssueVote> Votes { get; set; } = new List<IssueVote>();
        public ICollection<IssueShared> Shares { get; set; } = new List<IssueShared>(); 
        public ICollection<AiAnalysis> AiAnalyses { get; set; } =new List<AiAnalysis>();
        public ICollection<IssueAttachment> IssueAttachments { get; set; } = new List<IssueAttachment>();


    }
}
