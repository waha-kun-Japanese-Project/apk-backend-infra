using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class IssueTrackingStepDto
    {
        public IssueStatus Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public TrackingStepState State { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? Note { get; set; }

        public Guid? ExpertId { get; set; }
        public string? ExpertName { get; set; }
        public string? ExpertPhoto { get; set; }
        public string? RepairPhoto { get; set; }

    }
}
