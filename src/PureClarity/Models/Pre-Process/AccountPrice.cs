using System.Collections.Generic;
using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class AccountPrice : AccountPriceBase
    {       
        public string ParentId;
        public List<Price> Prices;
        public List<Price> SalePrices;
    }
}