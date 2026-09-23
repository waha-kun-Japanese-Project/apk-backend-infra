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
        string Status,
        string Priority,
        DateTime CreatedAt,
        Guid? AssignedExpertId)
        
    {
    }
}
