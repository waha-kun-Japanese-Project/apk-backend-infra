using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Client
{
    public record UploadFileResponse(string fileName, string filePath, string fileUrl)
    {
    }
}
