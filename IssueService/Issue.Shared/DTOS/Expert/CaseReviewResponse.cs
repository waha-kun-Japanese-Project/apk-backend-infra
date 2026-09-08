using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public  record  CaseReviewResponse(Guid Id,
        string Title,
        string? Description,
        IssueStatus Status,
        IssuePriority Priority,
        Guid ReporterId,
        Guid? AssignedExpertId,
        string Latitude,
        string Longitude,
        DateTime CreatedAt,
        IReadOnlyList<IssueAttachmentResponse> Attachments,
        AiAnalysisSummary? AiAnalysis,
        IReadOnlyList<ExpertReviewResponse> ExpertReviews)
    {
    }
}
