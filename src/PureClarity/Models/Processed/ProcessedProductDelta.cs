using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ProcessedProductDelta
    {
        public string AppKey { get; }
        public ProcessedProduct[] Products;
        public string[] DeleteProducts;
        public ProcessedProductDelta(string appKey)
        {
            AppKey = appKey;
        }
    }
}