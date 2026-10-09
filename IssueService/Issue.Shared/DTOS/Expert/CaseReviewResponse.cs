using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record CaseReviewResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public Guid ReporterId { get; init; }
        public Guid? AssignedExpertId { get; init; }
        public string Latitude { get; init; } = string.Empty;
        public string Longitude { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<IssueAttachmentResponse> Attachments { get; init; } = new List<IssueAttachmentResponse>();
        public AiAnalysisSummary? AiAnalysis { get; init; }
        public IReadOnlyList<ExpertReviewResponse> ExpertReviews { get; init; } = new List<ExpertReviewResponse>();
    }
}
