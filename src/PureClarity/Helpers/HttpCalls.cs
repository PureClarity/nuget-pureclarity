using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace PureClarity.Helpers
{
    public class HttpCalls
    {
        public static T Post<T>(string bodyToPost, string postUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var result = default(T);

                httpClient.Timeout = new TimeSpan(0, 0, 150);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(bodyToPost, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(postUrl, content, new CancellationToken(false));

                if (response.Result.IsSuccessStatusCode)
                {
                    result = JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
                }
                else
                {

                    //throw new Exception(string.Format("Request failure for object {0}: Status {1} returned for {2} with payload {3}", typeof(T), response.Result.StatusCode, postUrl, bodyToPost));
                }

                return result;
            }
        }

        public static T Get<T>(string getUrl, string queryString)
        {
            using (var httpClient = new HttpClient())
            {
                var result = default(T);

                httpClient.Timeout = new TimeSpan(0, 0, 150);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = httpClient.GetAsync(getUrl + queryString, new CancellationToken(false));

                if (response.Result.IsSuccessStatusCode)
                {
                    result = JSONSerialization.DeserializeJSONFromHttpResponse<T>(response);
                }
                else
                {

                }

                return result;
            }
        }
    }
}