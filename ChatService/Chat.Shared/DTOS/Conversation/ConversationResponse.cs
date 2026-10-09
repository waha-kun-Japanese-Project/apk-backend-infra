using Chat.Shared.DTOS.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Conversation
{
    public record  ConversationResponse(Guid Id, Guid IssueId, string  Status, DateTime StartedAt, DateTime? ClosedAt, ICollection<ParticipantResponse> Participants)
    {
    }
}
