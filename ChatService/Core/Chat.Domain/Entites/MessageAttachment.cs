using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public class MessageAttachment : BaseEntity<Guid>
    {
        public Guid MessageId { get; set; }

        public string FileName { get; set; } = null!;

        public string FileUrl { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }

        public AttachmentType Type { get; set; }

        public int? DurationSeconds { get; set; }

        public Message Message { get; set; } = null!;
    }
}
