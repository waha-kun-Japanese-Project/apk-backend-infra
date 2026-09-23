using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public class Message : BaseEntity<Guid>
    {
        public Guid ConversationId { get; set; }

        public Guid SenderId { get; set; }

        public MessageType Type { get; set; }

        public string? Content { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime? ReadAt { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public ICollection<MessageAttachment> Attachments { get; set; }
            = new List<MessageAttachment>();
    }
}
