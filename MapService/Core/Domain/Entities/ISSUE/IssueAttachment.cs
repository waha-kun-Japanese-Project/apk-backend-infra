using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities.ISSUE

{
    public class IssueAttachment : BaseEntity<Guid>
    {
        public IssueAttachmentType Type { get; set; }
        public string Url { get; set; } = null!;

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

    }
}
