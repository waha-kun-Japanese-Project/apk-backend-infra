using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Message
{
    public record MessageResponse(
    Guid Id,
    Guid ConversationId,
    Guid SenderId,
    string? Content,
    DateTime SentAt,
    DateTime? DeliveredAt,
    DateTime? ReadAt,
    IReadOnlyList<AttachmentResponse> Attachments)
    {
    }
}
