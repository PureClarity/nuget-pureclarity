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
        private static ConcurrentBag<string> _deletedProducts;
        private static ConcurrentBag<AccountPrice> _accountPrices;
        private static ConcurrentBag<DeletedAccountPrice> _deletedAccountPrices;
        private static ConcurrentBag<Category> _categories;
        private static ConcurrentBag<User> _users;

        [GlobalSetup]
        public static void GlobalSetup()
        {
            _products = new ConcurrentBag<Product>();
            _deletedProducts = new ConcurrentBag<string>();
            _accountPrices = new ConcurrentBag<AccountPrice>();
            _deletedAccountPrices = new ConcurrentBag<DeletedAccountPrice>();
            _categories = new ConcurrentBag<Category>();
            _users = new ConcurrentBag<User>();

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

                //Fake deleted products
                var testDeletedProduct = new Faker<string>()
                                   .CustomInstantiator(f => Guid.NewGuid().ToString());

                _deletedProducts.Add(testDeletedProduct);

                //Fake account prices
                var testAccountPrice = new Faker<AccountPrice>()
                                   .CustomInstantiator(f => new AccountPrice())
                                   .RuleFor(u => u.AccountId, f => f.Finance.Account())
                                   .RuleFor(u => u.Prices, f => new List<Price> { new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")) })
                                   .RuleFor(u => u.Sku, f => Guid.NewGuid().ToString());

                _accountPrices.Add(testAccountPrice);

                //Fake deleted account price
                var testDeletedAccountPrice = new Faker<DeletedAccountPrice>()
                                            .CustomInstantiator(f => new DeletedAccountPrice())
                                            .RuleFor(u => u.AccountId, f => f.Finance.Account())
                                            .RuleFor(u => u.Sku, f => Guid.NewGuid().ToString());

                _deletedAccountPrices.Add(testDeletedAccountPrice);

                //Create fake categories
                var testCategory = new Faker<Category>()
                                    .CustomInstantiator(f => new Category(Guid.NewGuid().ToString(),
                                    f.Commerce.Categories(1)[0],
                                    f.Internet.Url()));

                _categories.Add(testCategory);

                //Create fake categories
                var testUser = new Faker<User>()
                                    .CustomInstantiator(f => new User(Guid.NewGuid().ToString()))
                                    .RuleFor(u => u.City, f => f.Address.City())
                                    .RuleFor(u => u.Country, f => f.Address.CountryCode())
                                    .RuleFor(u => u.DOB, f => DateTime.Now.ToString("dd/MM/yyyy"))
                                    .RuleFor(u => u.Email, f => f.Person.Email)
                                    .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                                    .RuleFor(u => u.LastName, f => f.Person.LastName);

                _users.Add(testUser);
            });

            var tempTrue = false;
            var tempTrue2 = false;

            _products.AsParallel().ForAll((prod) =>
            {
                var attrs = new Faker<List<string>>().CustomInstantiator(f => new List<string> { f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial(), f.Commerce.ProductMaterial() });

                prod.Attributes.Add("Material", attrs.Generate());

                if (!tempTrue)
                {
                    tempTrue = true;
                    var testVariant = new Faker<Product>()
                                   .CustomInstantiator(f => new Product(Guid.NewGuid().ToString(),
                                   f.Commerce.ProductName(),
                                   f.Lorem.Paragraph(2),
                                   f.Internet.UrlWithPath(),
                                   f.Internet.Avatar(),
                                   f.Commerce.Categories(3).ToList()))
                                   .RuleFor(u => u.Prices, f => new List<Price> { new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "USD")) });

                    prod.Variants.Add(testVariant);
                    tempTrue = false;
                }
                else if (!tempTrue2)
                {
                    tempTrue2 = true;
                    var testVariant = new Faker<Product>()
                                   .CustomInstantiator(f => new Product(Guid.NewGuid().ToString(),
                                   f.Commerce.ProductName(),
                                   f.Lorem.Paragraph(2),
                                   f.Internet.UrlWithPath(),
                                   f.Internet.Avatar(),
                                   f.Commerce.Categories(3).ToList()))
                                   .RuleFor(u => u.Prices, f => new List<Price> { new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "USD")) });

                    prod.Variants.Add(testVariant);

                    var testVariant2 = new Faker<Product>()
                                   .CustomInstantiator(f => new Product(Guid.NewGuid().ToString(),
                                   f.Commerce.ProductName(),
                                   f.Lorem.Paragraph(2),
                                   f.Internet.UrlWithPath(),
                                   f.Internet.Avatar(),
                                   f.Commerce.Categories(3).ToList()))
                                   .RuleFor(u => u.Prices, f => new List<Price> { new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "GBP")), new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "USD")) });

                    prod.Variants.Add(testVariant2);

                    tempTrue2 = false;
                }
                else
                {
                    prod.Prices.Add(new Faker<Price>().CustomInstantiator(f => new Price(Decimal.Parse(f.Commerce.Price()), "GBP")));
                    prod.Prices.Add(new Faker<Price>().CustomInstantiator(g => new Price(Decimal.Parse(g.Commerce.Price()), "USD")));
                }
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
            feedManager.AddAccountPrices(_accountPrices);
            feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishProductFeedResult.Error}");
        }
        
        [Benchmark]
        public static void RunProductFeedVariantIssueCheck()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            var prod = new Product("59095",
            "Bear June Medium", 
            "<p>.</p>", 
            "souvenirs/novelties/bear-june-medium", 
            "siteUrl/filedepository/productimages/Souvenirs/novelties/bear-june-medium-1.jpg", 
            new List<string>{"Novelties > Souvenirs"});

            var variant = new Product("5909565",
            "Bear June Medium", 
            "<p>.</p>", 
            "souvenirs/novelties/bear-june-medium", 
            "siteUrl/filedepository/productimages/Souvenirs/novelties/bear-june-medium-1.jpg", 
            new List<string>{"Novelties > Souvenirs"});
            variant.Attributes.Add("Personalisable", new List<string>{"false"});
            variant.ParentId = "59095";


            var res1 = feedManager.AddProduct(variant);            
            var res2 = feedManager.AddProduct(prod);
            var valid = feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishProductFeedResult.Error}");
        }

        [Benchmark]
        public static void RunProductDeltas()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            feedManager.AddProducts(_products);
            feedManager.AddDeletedProductSkus(_deletedProducts);
            feedManager.AddAccountPrices(_accountPrices);
            feedManager.AddDeletedAccountPrices(_deletedAccountPrices);
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

            System.Threading.Tasks.Parallel.ForEach(_accountPrices, (accountPrice) =>
            {
                feedManager.AddAccountPrice(accountPrice);
            });

            var valid = feedManager.Validate();
            if (valid.Success)
            {
                var publishResult = feedManager.PublishAsync().Result;
                Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishProductFeedResult.Error}");
            }else{
                Console.WriteLine("Invalid feed");
            }
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

        [Benchmark]
        public static void RunUserFeed()
        {
            var feedManager = new FeedManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", "TEST", 0);
            feedManager.AddUsers(_users);
            var valid = feedManager.Validate();
            var publishResult = feedManager.PublishAsync().Result;
            Console.WriteLine($"Published: {publishResult.Success.ToString()}. Error: {publishResult.PublishUserFeedResult.Error}");
        }

        private static string GetFirstError(PureClarity.Models.ValidationResult validatorResult)
        {
            return validatorResult.InvalidRecords.Count != 0 ? validatorResult.InvalidRecords.First().Value.First() : String.Empty;
        }
    }

    public class QueryToken
    {
        public static void RunQueryTokens()
        {
            var tokenManager = new QueryTokenManager("7ad2d0bb-6c44-4a93-a146-6c8ed845860b", 0);
            var tokenResults = tokenManager.QueryTokensAsync(new List<string>{
                "Mhtf-R0LTASEHxHOfK0H8g",
                "8BwrRzGYSpSduVnhqZ8weQ",
                "_TIZzhg4TXSFn1W1YLeohw",
                "c_HPoSnRT4O1342N8M0ivQ",
                "2HCDBFIMT7mZjWk7L2QcCg",
                "x273bVGbRXK6T9eHBwCdjw"
                }).Result;
            Console.WriteLine($"Returned: {tokenResults.TokenStatuses.Count}. Error: {tokenResults.Error}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Feeds._itemCount = 1000;
            Feeds.GlobalSetup();
            Feeds.RunUserFeed();
            /*  //Runs a benchmark on all methods tagged with the [Benchmark] attribute and provides results at the end
             var summary = BenchmarkRunner.Run<Feeds>(); */
        }
    }
}
