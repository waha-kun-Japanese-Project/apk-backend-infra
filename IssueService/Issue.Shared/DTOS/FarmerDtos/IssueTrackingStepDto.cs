using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
