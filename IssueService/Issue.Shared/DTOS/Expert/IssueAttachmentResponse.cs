using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record IssueAttachmentResponse
    {
        public Guid Id { get; init; }
        public IssueAttachmentType Type { get; init; }
        public string Url { get; init; } = string.Empty;
    }
}
