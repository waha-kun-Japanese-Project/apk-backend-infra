using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public class ConversationParticipant : BaseEntity<Guid>
    {
        public Guid ConversationId { get; set; }

        public Guid UserId { get; set; }

        public ParticipantRole Role { get; set; }

        public DateTime JoinedAt { get; set; }

        public DateTime? LeftAt { get; set; }

        public DateTime? LastReadAt { get; set; }

        public Conversation Conversation { get; set; } = null!;
    }
}
