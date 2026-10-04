using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class IssueTrackingResponseDto
    {
        public Guid IssueId { get; set; }
        public IssueStatus CurrentStatus { get; set; }

        public List<IssueTrackingStepDto> Steps { get; set; }
            = new();
    }
}
