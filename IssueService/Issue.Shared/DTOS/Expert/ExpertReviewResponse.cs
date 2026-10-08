using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record ExpertReviewResponse
    {
        public Guid Id { get; init; }
        public ReviewDecision Decision { get; init; }
        public string? Notes { get; init; }
        public Guid ExpertId { get; init; }
        public DateTime ReviewedAt { get; init; }
    }
}
