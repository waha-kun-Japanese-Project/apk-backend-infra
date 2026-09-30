using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Message
{
    public record SendMessageRequest(Guid ConversationId, string Content)
    {
    }
}
