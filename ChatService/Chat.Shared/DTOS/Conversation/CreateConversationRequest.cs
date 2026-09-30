using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Conversation
{
    public  record CreateConversationRequest( Guid IssueId, Guid ExpertId)
    {
    }
}
