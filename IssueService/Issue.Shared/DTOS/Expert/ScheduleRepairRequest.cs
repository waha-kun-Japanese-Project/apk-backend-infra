using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record ScheduleRepairRequest(
       DateOnly ScheduledDate,
       TimeOnly SlotStart,
       TimeOnly SlotEnd,
       Guid? TeamId,
       bool FarmerNotified,
       string? Notes)
    {
    }
}
