using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Entities
{
    public class IssueAttachment : Basentity<Guid>
    {
        public IssueAttachmentType Type { get; set; }
        public string Url { get; set; } = null!;

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;

    }
}
