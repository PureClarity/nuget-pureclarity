using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class AccountPrice : PCModelBase
    {
        public string AccountId { get => Id.Split('_')[0]; set => Id = $"{value}_{Sku}"; }
        public string Sku { get => Id.Split('_')[1]; set => Id = $"{AccountId}_{value}"; }
        public string ParentId;
        public string[] Prices;
        public string[] SalePrices;
    }
}