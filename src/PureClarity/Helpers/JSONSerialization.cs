using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PureClarity.Helpers
{
    public class JSONSerialization
    {
        public static string SerializeToJSON<T>(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static T DeserializeJSONFromHttpResponse<T>(Task<HttpResponseMessage> response)
        {
            T result;

            string s = response.Result.Content.ReadAsStringAsync().Result;

            result = JsonConvert.DeserializeObject<T>(s.ToString());

            return result;
        }

        public static int GetByteSizeOfObject<T>(T objectToSerialize)
        {
            var serializedObject = SerializeToJSON(objectToSerialize);
            return serializedObject.Length * sizeof(char);
        }
    }
}