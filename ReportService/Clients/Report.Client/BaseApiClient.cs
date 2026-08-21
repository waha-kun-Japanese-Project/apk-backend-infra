using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Report.Client
{
    public abstract class BaseApiClient(HttpClient httpClient)
    {
        protected async Task <TResponse>GetAsync<TResponse>(string url, CancellationToken cancellationToken = default)
        { 
            var respones =await httpClient.GetAsync(url, cancellationToken);
            respones.EnsureSuccessStatusCode();
            return await respones.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken)??
                throw new Exception ("Failed to deserialize response.");

        }
        protected async Task<TResponse> PostAsync<TResponse>(string url, HttpContent content, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ??
                throw new Exception("Failed to deserialize response.");
        }
        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string url, HttpContent content, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PutAsJsonAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ??
                throw new Exception("Failed to deserialize response.");
        }
        protected async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.DeleteAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
        }


    }
}
