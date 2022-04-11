using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallApi
{
    public class ApiClient: IDisposable
    {
        internal readonly HttpClient _client;
        public ApiClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }
        public async Task<HttpResponseMessage> SuccessfulGetRequest(string urlAddition)
        { 
            HttpResponseMessage response = await _client.GetAsync(urlAddition);
            if (response.IsSuccessStatusCode)
                return response;
            else
                throw new Exception($"Unsuccessful Call\n" +
                    $"ErrorMessage: {await response.Content.ReadAsStringAsync()}");
        }
        public async Task<HttpResponseMessage> SuccessfulPostRequest(string urlAddition, HttpContent content)
        {
            HttpResponseMessage response = await _client.PostAsync(urlAddition, content);
            if (response.IsSuccessStatusCode)
                return response;
            else
                throw new Exception($"Unsuccessful Call\n" +
                    $"ErrorMessage: {await response.Content.ReadAsStringAsync()}");
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
