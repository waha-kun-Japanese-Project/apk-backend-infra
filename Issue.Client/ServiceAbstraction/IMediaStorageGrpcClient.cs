using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Client.ServiceAbstraction
{
    public interface IMediaStorageGrpcClient
    {
        Task<string> UploadAsync( IFormFile file,CancellationToken cancellationToken = default);
    }
}
