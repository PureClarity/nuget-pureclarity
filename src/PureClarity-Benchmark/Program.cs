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
    public class Feeds
    {
        [Params(1000, 5000, 10000, 25000, 50000, 100000)]
        public static int _itemCount;

        private static ConcurrentBag<Product> _products;
        private static ConcurrentBag<Category> _categories;

        [GlobalSetup]
        public static void GlobalSetup()
        {
            _products = new ConcurrentBag<Product>();
            _categories = new ConcurrentBag<Category>();

            System.Threading.Tasks.Parallel.For(0, _itemCount, (val) =>
             {
                 //Create fake products
                 var testProduct = new Faker<Product>()
                                    .CustomInstantiator(f => new Product(Guid.NewGuid().ToString(),
                                    f.Commerce.ProductName(),
                                    f.Lorem.Paragraph(2),
                                    f.Internet.UrlWithPath(),
                                    f.Internet.Avatar(),
                                    f.Commerce.Categories(3).ToList()));

                 _products.Add(testProduct);


                 //Create fake categories
                 var testCategory = new Faker<Category>()
                                     .CustomInstantiator(f => new Category(Guid.NewGuid().ToString(),
                                     f.Commerce.Categories(1)[0],
                                     f.Internet.Url()));

                 _categories.Add(testCategory);
             });

            _products.AsParallel().ForAll((prod) =>
            {
                prod.Prices.Add(new Faker<ProductPrice>().CustomInstantiator(f => new ProductPrice(Decimal.Parse(f.Commerce.Price()), "GBP")));

                var attrs = new Faker<IEnumerable<string>>().CustomInstantiator(f => new List<string> { f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial() });

                prod.Attributes.Add("Material", attrs.Generate());
            });

            var parentCats = new ConcurrentBag<string>();

            _categories.AsParallel().ForAll((cat) =>
            {
                if (parentCats.Count() < 20)
                {
                    parentCats.Add(cat.Id);
                }
                else
                {
                    var parents = new Faker<IEnumerable<string>>().CustomInstantiator(f => new List<string> { f.PickRandom(parentCats.ToList()) });
                    cat.ParentIds = parents.Generate().ToArray();
                }
            });

            Console.WriteLine($"{_products.Count()}, {_categories.Count()}");
        }

        [Benchmark]
        public static void RunProductFeed()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            feedManager.AddProducts(_products);
            feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishProductFeedResult.Error}");
        }

        [Benchmark]
        public static void RunProductDeltas()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            feedManager.AddProducts(_products);
            feedManager.Validate();
            var publishResult = feedManager.PublishDeltasAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.Errors.Count}");
        }

        [Benchmark]
        public static void RunParallelAddProductFeed()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);

            System.Threading.Tasks.Parallel.ForEach(_products, (prod) =>
            {
                feedManager.AddProduct(prod);
            });

            feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishProductFeedResult.Error}");
        }

        [Benchmark]
        public static void RunCategoryFeed()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            feedManager.AddCategories(_categories);
            feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishCategoryFeedResult.Error}");
        }

        private static string GetFirstError(PureClarity.Models.ValidationResult validatorResult)
        {
            return validatorResult.InvalidRecords.Count != 0 ? validatorResult.InvalidRecords.First().Value.First() : String.Empty;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Feeds._itemCount = 1000;
            Feeds.GlobalSetup();
            
            //Runs a benchmark on all methods tagged with the [Benchmark] attribute and provides results at the end
            var summary = BenchmarkRunner.Run<Feeds>();
        }
    }
}
