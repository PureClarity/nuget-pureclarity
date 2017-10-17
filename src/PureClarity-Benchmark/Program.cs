using System;
using System.Collections.Generic;
using Bogus;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PureClarity.Managers;
using PureClarity.Models;
using System.Linq;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System.Collections.Concurrent;

namespace PureClarity_Benchmark
{

    [SimpleJob(RunStrategy.Monitoring)]
    public class ProductFeed
    {
        [Params(1000, 5000, 10000, 25000, 50000, 100000)]
        public static int _prodCount;

        private static ConcurrentBag<Product> _products;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _products = new ConcurrentBag<Product>();
            System.Threading.Tasks.Parallel.For(0, _prodCount, (val) =>
             {
                 var testProduct = new Faker<Product>()
                                    .CustomInstantiator(f => new Product(Guid.NewGuid().ToString(),
                                    f.Commerce.ProductName(),
                                    f.Lorem.Paragraph(2),
                                    f.Internet.UrlWithPath(),
                                    f.Internet.Avatar(),
                                    f.Commerce.Categories(3)));

                 _products.Add(testProduct);
             });

            Console.WriteLine(_products.Count());
        }

        [Benchmark]
        public static void RunParallelGenBatchInsertBenchmark()
        {
            var feedManager = new FeedManager("api.pureclarity.dev", "7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "");
            feedManager.AddProducts(_products);
            Console.WriteLine(feedManager.GetProductCollectionState().ItemCount);
        }

        [Benchmark]
        public static void RunParallelGenAndInsertBenchmark()
        {
            var feedManager = new FeedManager("api.pureclarity.dev", "7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "");

            System.Threading.Tasks.Parallel.ForEach(_products, (prod) =>
            {
                feedManager.AddProduct(prod);
            });

            Console.WriteLine(feedManager.GetProductCollectionState().ItemCount);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //ProductFeed.RunBenchmark();
            var summary = BenchmarkRunner.Run<ProductFeed>();
        }
    }
}
