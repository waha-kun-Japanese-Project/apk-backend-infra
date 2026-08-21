//using Microsoft.AspNetCore.Http;
//using Report.Client.AbstractServices;
//using Report.Shared.DTOS.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Report.Client.Services
//{
//    public class StorageClient(HttpClient httpClient) :  BaseApiClient(httpClient), IStorageClient
//    {
//        public Task<Stream> DownloadAsync(string objectName, CancellationToken cancellationToken = default)
//        {
//            return base.GetAsync<Stream>($"api/storage/download?objectName={Uri.EscapeDataString(objectName)}", cancellationToken);

//        }

//        public async Task<UploadFileResponse> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
//        {
//            //Content-Type : multipart/form-data or json or application/x-www-form-urlencoded
//            using var form  = new MultipartFormDataContent();
//            form.Add( new StreamContent(file.OpenReadStream()),"file",file.FileName);

//            return await PostAsync<UploadFileResponse>($"api/storage/upload?folder={folder}", form, cancellationToken);


//        }

//       public async Task DeleteAsync(string objectName, CancellationToken cancellationToken)
//        {
//            await base.DeleteAsync(
//              $"api/storage/{Uri.EscapeDataString(objectName)}",
//              cancellationToken);
//        }
//    }
//}
