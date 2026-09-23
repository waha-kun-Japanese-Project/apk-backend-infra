using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record IssueAttachmentResponse(
        Guid Id,
        IssueAttachmentType Type,
        string Url
    );
}
