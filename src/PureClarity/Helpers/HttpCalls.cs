using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PureClarity.Models.Response;

namespace PureClarity.Helpers
{
    internal class HttpCalls
    {
        public static async Task<T> Post<T>(string bodyToPost, string postUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var result = default(T);

                httpClient.Timeout = new TimeSpan(0, 0, 150);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(bodyToPost, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(postUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
                }
                else
                {
                    throw new Exception($"Request failure for object {typeof(T)}: POST Status {response.StatusCode} with body {await response.Content.ReadAsStringAsync()} returned for {postUrl} with payload {bodyToPost}");
                }

                return result;
            }
        }

        public static async Task<T> Get<T>(string getUrl, string queryString)
        {
            using (var httpClient = new HttpClient())
            {
                var result = default(T);

                httpClient.Timeout = new TimeSpan(0, 0, 150);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync(getUrl + queryString);

                if (response.IsSuccessStatusCode)
                {
                    result = await JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
                }
                else
                {
                    throw new Exception($"Request failure for object {typeof(T)}: GET Status {response.StatusCode} returned for {getUrl} with query string {queryString}");
                }

                return result;
            }
        }
    }
}