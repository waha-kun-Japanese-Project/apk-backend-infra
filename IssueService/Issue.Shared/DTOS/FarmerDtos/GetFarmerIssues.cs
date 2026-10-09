using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class GetFarmerIssues
    {
        public Guid IssueId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IssueStatus Status { get; set; }
        public IssuePriority priority { get; set; }
        public string longitude { get; set; }
        public string Latiude { get; set; }
        public Guid ReporterId { get; set; }
        public string ExpertName { get; set; } = null!;
        public string ExpertUrl { get; set; }
        public Guid ExpertId { get; set; }
    }
}
