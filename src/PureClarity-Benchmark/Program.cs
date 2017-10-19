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
using Renci.SshNet;
using System.Threading.Tasks;

namespace PureClarity_Benchmark
{

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring)]
    public class ProductFeed
    {
        [Params(1000, 5000, 10000, 25000, 50000, 100000)]
        public static int _prodCount;

        private static ConcurrentBag<Product> _products;

        [GlobalSetup]
        public static void GlobalSetup()
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
                                    f.Commerce.Categories(3).ToList()));

                 _products.Add(testProduct);
             });

            _products.AsParallel().ForAll((prod) =>
            {
                prod.Prices.Add(new Faker<ProductPrice>().CustomInstantiator(f => new ProductPrice(Decimal.Parse(f.Commerce.Price()), "GBP")));

                var attrs = new Faker<IEnumerable<string>>().CustomInstantiator(f => new List<string> { f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial() });

                prod.Attributes.Add("Material", attrs.Generate());
            });

            Console.WriteLine(_products.Count());
        }

        [Benchmark]
        public static void RunSequentialBenchmark()
        {
            var feedManager = new FeedManager("api.pureclarity.dev", "7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", "");
            feedManager.AddProducts(_products);
            feedManager.Validate();
            var publishResult = feedManager.PublishProductFeed().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.Error}");
        }

        [Benchmark]
        public static void RunParallelBenchmark()
        {
            var feedManager = new FeedManager("api.pureclarity.dev", "7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", "");

            System.Threading.Tasks.Parallel.ForEach(_products, (prod) =>
            {
                feedManager.AddProduct(prod);
            });

            feedManager.Validate();
            var publishResult = feedManager.PublishProductFeed().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.Error}");
        }

        private static string GetFirstError(ValidatorResult validatorResult)
        {
            return validatorResult.InvalidRecords.Count != 0 ? validatorResult.InvalidRecords.First().Value.First() : String.Empty;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            ProductFeed._prodCount = 10000;
            ProductFeed.GlobalSetup();
            ProductFeed.RunSequentialBenchmark();
            //var summary = BenchmarkRunner.Run<ProductFeed>();
            /* var connectionInfo = new ConnectionInfo("localhost", 2222,
                                                    "7ad2d0bb-6c44-4a93-a146-6c8ed845860b",
                                                    new[] { new PasswordAuthenticationMethod("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST") });
            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();
                Console.WriteLine(client.ConnectionInfo.ServerVersion);
            } */
        }
    }
}
