using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public  class RepairSchedule:BaseEntity<Guid>
    {
        public DateOnly ScheduledDate { get; set; }
        public TimeOnly SlotStart { get; set; }
        public TimeOnly SlotEnd { get; set; }
        public bool FarmerNotified { get; set; }
        public string? Notes { get; set; }

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

        public Guid? TeamId { get; set; }
        public MaintenanceTeam? Team { get; set; }
    }
}
