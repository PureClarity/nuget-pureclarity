using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PureClarity.Helpers
{
    internal static class HttpCalls
    {
        private static readonly HttpClient httpClient = CreateClient();

        private static HttpClient CreateClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(150) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async Task<T> Post<T>(string bodyToPost, string postUrl)
        {
            using (var content = new StringContent(bodyToPost, Encoding.UTF8, "application/json"))
            using (var response = await httpClient.PostAsync(postUrl, content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failure for object {typeof(T)}: POST to {postUrl} returned status {(int)response.StatusCode} ({response.StatusCode}).");
                }

                return await JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
            }
        }

        public static async Task<T> Get<T>(string getUrl, string queryString)
        {
            using (var response = await httpClient.GetAsync(getUrl + queryString))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failure for object {typeof(T)}: GET to {getUrl} returned status {(int)response.StatusCode} ({response.StatusCode}).");
                }

                return await JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
            }
        }
    }
}