using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared.DTOS.Client
{
    public  record MediaUploadResponse(string FileName,
    string FileUrl,
    string ContentType,
    long FileSize)
    {
    }
}
