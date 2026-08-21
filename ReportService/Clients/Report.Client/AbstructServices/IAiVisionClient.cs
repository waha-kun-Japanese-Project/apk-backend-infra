using Microsoft.AspNetCore.Http;
using Refit;
using Report.Shared.DTOS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Client.AbstructServices
{
    public  interface IAiVisionClient
    {
        [Multipart]
        [Post("/api/v1/predict")]
        Task<AiPredictionResponse> PredictAsync(
        [AliasAs("file")] StreamPart image);
    }
}
