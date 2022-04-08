using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CallToDoApi
{
    public class ApiClient: IDisposable
    {
        private readonly HttpClient _client;
        public ApiClient(string uri, string defaultHeader)
        { 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(uri);
            //figure out how to get defaultHeader to work without having to give
            //people instructions
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
