using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Message
{
    public  record AttachmentResponse(
         Guid Id,
    string FileName,
    string FileUrl,
    string ContentType,
    long FileSize,
    string AttachmentType,
    double? DurationSeconds
        )
    {
    }
}
