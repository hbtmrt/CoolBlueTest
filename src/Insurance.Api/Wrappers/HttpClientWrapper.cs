using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Insurance.Api.Wrappers
{
    public sealed class HttpClientWrapper
    {
        private readonly Uri baseUri;

        public HttpClientWrapper(Uri uri)
        {
            baseUri = uri;
        }

        public T Get<T>(string uriPath)
        {
            HttpClient client = new HttpClient { BaseAddress = baseUri };
            string json = client.GetAsync(uriPath).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}