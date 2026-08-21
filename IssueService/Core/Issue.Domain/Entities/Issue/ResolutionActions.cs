using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  class ResolutionActions:BaseEntity<Guid>
    {
        public string ActionType { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public Guid? TeamId { get; set; }
        public MaintenanceTeam? Team { get; set; }

    }
}
