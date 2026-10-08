using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record RepairScheduleResponse
    {
        public Guid Id { get; init; }
        public Guid IssueId { get; init; }
        public DateOnly ScheduledDate { get; init; }
        public TimeOnly SlotStart { get; init; }
        public TimeOnly SlotEnd { get; init; }
     
        public bool FarmerNotified { get; init; }
        public string? Notes { get; init; }
        public string Status { get; init; }
    }
}
