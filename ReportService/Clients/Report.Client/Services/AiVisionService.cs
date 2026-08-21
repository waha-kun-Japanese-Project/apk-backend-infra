//using Microsoft.AspNetCore.Http;
//using Report.Client.AbstructServices;
//using Report.Shared.DTOS.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Report.Client.Services
//{
//    public class AiVisionClient(HttpClient httpClient) : BaseApiClient(httpClient), IAiVisionClient
//    {
//        public async Task<AiPredictionResponse> PredictAsync(IFormFile image, CancellationToken cancellationToken = default)
//        {
//            var content = new MultipartFormDataContent();
//            content.Add(
//             new StreamContent(image.OpenReadStream()),
//             "file",
//             image.FileName);

//            return await PostAsync<AiPredictionResponse>("predict",content,cancellationToken)
//                ?? throw new Exception("Prediction failed.");

//        }
//    }
//}
