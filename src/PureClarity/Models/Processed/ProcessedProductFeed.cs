using System.Collections.Generic;
using PureClarity.Models.Processed;

namespace PureClarity.Models
{
    public class ProcessedProductFeed
    {
        public List<ProcessedProduct> Products;
        public List<ProcessedAccountPrice> AccountPrices;

        public ProcessedProductFeed()
        {
            Products = new List<ProcessedProduct>();
            AccountPrices = new List<ProcessedAccountPrice>();
        }
    }
}