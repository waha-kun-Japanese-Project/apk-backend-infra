using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Shared.DTOS
{
    public record UploadStreamRequest(Stream Stream, string fileName,
    string contentType,
    string folder)
    {
    }
}
