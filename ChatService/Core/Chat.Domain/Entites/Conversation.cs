using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public class Conversation : BaseEntity<Guid>
    {
        public Guid IssueId { get; set; }

        public ConversationStatus Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public ICollection<ConversationParticipant> Participants { get; set; }
            = new List<ConversationParticipant>();

        public ICollection<Message> Messages { get; set; }
            = new List<Message>();
    }
}
