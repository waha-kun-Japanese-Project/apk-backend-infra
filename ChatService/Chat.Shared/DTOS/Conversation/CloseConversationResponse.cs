using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Conversation
{
    public  record CloseConversationResponse(
        Guid ConversationId,
        string Status ,
        DateTime ClosedAt
        )
    {
    }
}
