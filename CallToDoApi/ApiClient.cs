using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CallToDoApi
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        public ApiClient(string uri, string defaultRequestHeader)
        { 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(uri);
        }
    }
}
