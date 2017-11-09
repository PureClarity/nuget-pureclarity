using System.Collections.Generic;
using PureClarity.Models.Processed;

namespace PureClarity.Models
{
    internal class ProcessedProductDelta
    {
        public string AppKey { get; }
        public ProcessedProduct[] Products;
        public string[] DeleteProducts;
        public ProcessedAccountPrice[] AccountPrices;
        public DeletedAccountPrice[] DeleteAccountPrices;

        public ProcessedProductDelta(string appKey)
        {
            AppKey = appKey;
        }
    }
}