using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record  ExpertInboxResponse(
        Guid Id,
        string Title,
        string? Description,
        IssueStatus Status,
        IssuePriority Priority,
        string? ThumbnailUrl,
        DateTime CreatedAt,
        Guid? AssignedExpertId,
        string FullName)
    {
    }
}
