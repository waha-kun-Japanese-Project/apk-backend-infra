using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record SubmitExpertReviewResponse
    {
        public Guid ReviewId { get; init; }
        public Guid IssueId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Notes { get; init; } = string.Empty;
        public string Decision { get; init; } = string.Empty;
    }
}
