using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PureClarity.Helpers
{
    internal class JSONSerialization
    {
        public static string SerializeToJSON<T>(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static async Task<T> DeserializeJSONFromHttpResponse<T>(HttpResponseMessage response)
        {
            T result;

            string s = await response.Content.ReadAsStringAsync();

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