using Microsoft.AspNetCore.Http;
using Refit;
using Report.Shared.DTOS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Report.Client.AbstructServices
{
    public interface IStorageClient
    {
        [Multipart]
        [Post("/api/storage/upload")]
        Task<UploadFileResponse> UploadAsync(
      [AliasAs("file")] StreamPart file,
       [Query]
        string folder);

        [Delete("/api/storage")]
        Task DeleteAsync(string objectName);
        [Get("/api/storage")]
        Task<Stream> DownloadAsync([AliasAs("objectName")] string objectName);


    }
}
