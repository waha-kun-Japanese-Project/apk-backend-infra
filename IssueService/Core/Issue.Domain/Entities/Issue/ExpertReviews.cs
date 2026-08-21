using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  class ExpertReviews: BaseEntity<Guid>
    {
        public ReviewDecision Decision { get; set; }
        public string? Notes { get; set; }
        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public Guid ExpertId { get; set; }
      
    }
}
