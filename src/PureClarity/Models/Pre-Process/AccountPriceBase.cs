using System.Collections.Generic;
using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class AccountPriceBase : PCModelBase
    {
        public string AccountId { get => Id?.Split('|')[0]; set => Id = $"{value}|{Sku}"; }
        public string Sku { get => Id?.Split('|')[1]; set => Id = $"{AccountId}|{value}"; }
    }
}