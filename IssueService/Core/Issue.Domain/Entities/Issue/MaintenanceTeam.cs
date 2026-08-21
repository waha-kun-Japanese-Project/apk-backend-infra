using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Issue
{
    public class MaintenanceTeam:BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<ResolutionActions> ResolutionActions { get; set; } = new List<ResolutionActions>();
        public ICollection<RepairSchedule> RepairSchedules { get; set; } = new List<RepairSchedule>();
    }
}
