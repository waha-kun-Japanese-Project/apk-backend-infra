using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  class StatusHistory: BaseEntity<Guid>
    {
        public IssueStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public Guid ChangedById { get; set; } // User who changed the status


    }
}
