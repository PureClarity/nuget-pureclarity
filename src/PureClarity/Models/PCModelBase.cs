using Newtonsoft.Json;

namespace PureClarity.Models
{
    public abstract class PCModelBase
    {
        [JsonIgnore]
        public string Id;
    }
}