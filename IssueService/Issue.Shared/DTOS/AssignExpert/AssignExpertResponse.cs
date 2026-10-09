using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.AssignExpert
{
    public record AssignExpertResponse
    {
        public Guid IssueId { get; init; }
        public Guid AssignedExpertId { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
