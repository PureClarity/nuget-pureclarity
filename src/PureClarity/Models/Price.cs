using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class Price: PCModelBase
    {        
        public string Sku { get => Id; set => Id = value; }
        public string ParentId;
        public string[] Prices;
        public string[] SalePrices;
    }
}