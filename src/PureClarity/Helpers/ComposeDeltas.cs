using System;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity.Helpers
{
    public class ComposeDeltas
    {
        const int _maxDeltaSize = 250000;

        public static List<ProcessedProductDelta> GenerateDeltas(IEnumerable<ProcessedProduct> processedProducts, string appKey)
        {
            var validDeltas = new List<ProcessedProductDelta>();
            var deltasValid = false;
            var productsPerDelta = 5000;

            while (!deltasValid)
            {
                validDeltas.Clear();
                var productPartitions = Partition(processedProducts, productsPerDelta);

                deltasValid = !productPartitions.Any((partition) =>
                {
                    var feed = new ProcessedProductDelta(appKey);
                    feed.Products = partition.ToArray();
                    validDeltas.Add(feed);
                    var deltaTotal = JSONSerialization.GetByteSizeOfObject(feed);
                    return deltaTotal < _maxDeltaSize;
                });

                if (!deltasValid)
                {
                    productsPerDelta = productsPerDelta / 5;
                    if (productsPerDelta < 8)
                    {
                        throw new Exception("Individual products are too large to fit into a delta to send to PureClarity. Please reduce the amount of data been sent per product.");
                    }
                }
            }

            return validDeltas;
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(IEnumerable<T> sequence, int size)
        {
            List<T> partition = new List<T>(size);
            foreach (var item in sequence)
            {
                partition.Add(item);
                if (partition.Count == size)
                {
                    yield return partition;
                    partition = new List<T>(size);
                }
            }
            if (partition.Count > 0)
                yield return partition;
        }
    }
}