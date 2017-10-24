using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class DeletedAccountPrice : PCModelBase
    {
        public string AccountId { get => Id.Split('_')[0]; set => Id = $"{value}_{Sku}"; }
        public string Sku { get => Id.Split('_')[1]; set => Id = $"{AccountId}_{value}"; }
    }
}