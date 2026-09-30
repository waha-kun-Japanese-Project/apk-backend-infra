using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Participant
{
    public  record ParticipantResponse(Guid UserId,
    string? Name,
    string? ProfileImageUrl,
    string? Role)
    {
    }
}
