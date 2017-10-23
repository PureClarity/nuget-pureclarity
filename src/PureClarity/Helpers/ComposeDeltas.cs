using System;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity.Helpers
{
    public class ComposeDeltas
    {
        const int _maxDeltaSize = 250000;

        public static List<ProcessedProductDelta> GenerateDeltas(IEnumerable<ProcessedProduct> processedProducts, IEnumerable<DeletedProductSku> deletedProducts, string appKey)
        {
            var validProductDeltas = GenerateProductDeltas(processedProducts, appKey);
            var validDeletedProductDeltas = GenerateDeletedProductDeltas(deletedProducts, appKey);

            return validProductDeltas.Concat(validDeletedProductDeltas).ToList();
        }

        private static List<ProcessedProductDelta> GenerateProductDeltas(IEnumerable<ProcessedProduct> processedProducts, string appKey)
        {
            var validProductDeltas = new List<ProcessedProductDelta>();
            var deltasValid = false;
            var productsPerDelta = 5000;

            while (!deltasValid)
            {
                validProductDeltas.Clear();
                var productPartitions = Partition(processedProducts, productsPerDelta);

                deltasValid = productPartitions.All((partition) =>
                {
                    var feed = new ProcessedProductDelta(appKey);
                    feed.Products = partition.ToArray();
                    validProductDeltas.Add(feed);
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

            return validProductDeltas;
        }

        private static List<ProcessedProductDelta> GenerateDeletedProductDeltas(IEnumerable<DeletedProductSku> deletedProducts, string appKey)
        {
            var validDeletedProductDeltas = new List<ProcessedProductDelta>();
            var deltasValid = false;
            var productsPerDelta = 5000;

            while (!deltasValid)
            {
                validDeletedProductDeltas.Clear();
                var deletedProductPartitions = Partition(deletedProducts, productsPerDelta);

                deltasValid = deletedProductPartitions.All((partition) =>
                {
                    var feed = new ProcessedProductDelta(appKey);
                    feed.DeleteProducts = partition.Select((deletedProduct) => deletedProduct.Sku).ToArray();
                    validDeletedProductDeltas.Add(feed);
                    var deltaTotal = JSONSerialization.GetByteSizeOfObject(feed);
                    return deltaTotal < _maxDeltaSize;
                });

                if (!deltasValid)
                {
                    productsPerDelta = productsPerDelta / 5;
                    if (productsPerDelta < 8)
                    {
                        throw new Exception("Individual deleted product skus are too large to fit into a delta to send to PureClarity. You may have the wrong data in your sku");
                    }
                }
            }

            return validDeletedProductDeltas;
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