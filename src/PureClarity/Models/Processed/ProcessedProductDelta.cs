using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ProcessedProductDelta
    {
        public string AppKey { get; }
        public ProcessedProduct[] Products;
        public ProcessedProductDelta(string appKey)
        {
            AppKey = appKey;
        }
    }
}