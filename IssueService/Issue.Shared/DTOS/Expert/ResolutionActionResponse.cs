using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record ResolutionActionResponse(
       Guid Id,
       Guid IssueId,
       string ActionType,
       string? Notes,
       Guid? TeamId,
       string? TeamName,
       IssueStatus Status,
       DateTime CreatedAt)
    {
    }
}
