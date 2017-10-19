using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ProcessedProductFeed
    {
        public List<ProcessedProduct> Products;

        public ProcessedProductFeed()
        {
            Products = new List<ProcessedProduct>();
        }
    }
}