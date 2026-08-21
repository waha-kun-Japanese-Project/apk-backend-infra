using Media.Shared.DTOS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.ServiceAbstraction
{

    public interface IStorageService
    {
        Task <UploadFileResponse> UploadFileAsync(IFormFile file,string folder, CancellationToken cancellationToken = default);
        Task DeleteAsync(string objectName, CancellationToken cancellationToken = default);
        Task<Stream> DownloadAsync( string objectName, CancellationToken cancellationToken = default);
        Task<FileResponse> GetAsync(string objectName, CancellationToken cancellationToken = default);

    }
}
