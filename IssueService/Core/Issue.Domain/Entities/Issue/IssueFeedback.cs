using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  class IssueFeedback:BaseEntity<Guid>
    {
        public int Rating { get; set; } // e.g. 1-5 stars
        public string? Comment { get; set; }

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;
    }
}
