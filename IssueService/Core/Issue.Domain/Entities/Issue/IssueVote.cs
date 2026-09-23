using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public class IssueVote : BaseEntity<Guid>
    {
        public Guid IssueId { get; set; }

        public Issue Issue { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}
