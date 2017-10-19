using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using PureClarity.Models;

namespace PureClarity.Managers
{
    public class ConversionManager
    {
        public static ProcessedProductFeed ProcessProductFeed(IEnumerable<Product> preProcessProducts)
        {
            var feed = new ProcessedProductFeed();
            foreach (var product in preProcessProducts)
            {
                ProcessedProduct processedProduct = ConvertProduct(product);
                feed.Products.Add(processedProduct);
            }
            return feed;
        }

        private static ProcessedProduct ConvertProduct(Product product)
        {
            var processedProduct = new ProcessedProduct
            {
                AccountExclusions = product.AccountExclusions?.ToArray(),
                AccountInclusions = product.AccountInclusions?.ToArray(),
                Brand = product.Brand,
                Categories = product.Categories.ToArray(),
                Description = product.Description,
                ExcludeFromRecommenders = product.ExcludeFromRecommenders,
                Image = product.Image,
                ImageOverlay = product.ImageOverlay,
                Link = product.Link,
                NewArrival = product.NewArrival,
                NoDefaultPriceForAccounts = product.NoDefaultPriceForAccounts,
                OnOffer = product.OnOffer,
                SearchTags = product.SearchTags?.ToArray(),
                Sku = product.Sku,
                Title = product.Title,
                AssociatedTitles = new[] { product.Title }
            };

            var associatedSkus = new List<string> { product.Sku };
            var associatedTitles = new List<string> { product.Title };
            var prices = product.Prices.Count != 0 ? product.Prices : new List<ProductPrice>();
            var salePrices = product.SalePrices.Count != 0 ? product.SalePrices : new List<ProductPrice>();

            foreach (var variant in product.Variants)
            {
                associatedSkus.Add(variant.Sku);
                associatedTitles.Add(variant.Title);
                prices.AddRange(variant.Prices);
                salePrices.AddRange(variant.SalePrices);
            }

            processedProduct.Prices = ProcessPrices(prices);
            processedProduct.SalePrices = ProcessPrices(salePrices);

            var attributes = new Dictionary<string, JToken>();
            foreach (var attr in product.Attributes)
            {
                var values = attr.Value.Select((value) => { return WebUtility.HtmlEncode(value); });
                attributes.Add(WebUtility.HtmlEncode(attr.Key), new JArray(values));
            }

            processedProduct.Attributes = attributes;
            return processedProduct;
        }

        private static string[] ProcessPrices(List<ProductPrice> prices)
        {
            var minMaxedPrices = prices.GroupBy((price) => price.Currency)
            .Select((currencyGrouping) =>
            {
                var orderedPrices = currencyGrouping.OrderBy((price) => { return price.Price; });
                return new List<ProductPrice> { orderedPrices.First(), orderedPrices.Last() };
            })
            .SelectMany((minMaxedCurrencyGroupings) => minMaxedCurrencyGroupings)
            .Distinct();

            var convertedPrices = new List<string>();

            foreach (var price in minMaxedPrices)
            {
                convertedPrices.Add($"{price.Price} {price.Currency}");
            }

            return convertedPrices.ToArray();
        }
    }
}