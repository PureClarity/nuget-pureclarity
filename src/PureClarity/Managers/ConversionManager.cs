using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using PureClarity.Helpers;
using PureClarity.Models;
using PureClarity.Models.Processed;

namespace PureClarity.Managers
{
    internal class ConversionManager
    {
        public static ProcessedProductFeed ProcessProductFeed(IEnumerable<Product> preProcessProducts, IEnumerable<AccountPrice> accountPrices)
        {
            var feed = new ProcessedProductFeed();
            foreach (var product in preProcessProducts)
            {
                ProcessedProduct processedProduct = ConvertProduct(product);
                feed.Products.Add(processedProduct);
            }

            foreach (var price in accountPrices)
            {
                ProcessedAccountPrice processedAccountPrice = ConvertAccountPrice(price);
                feed.AccountPrices.Add(processedAccountPrice);
            }

            return feed;
        }

        public static List<ProcessedProductDelta> ProcessProductDeltas(IEnumerable<Product> preProcessProducts, IEnumerable<DeletedProductSku> deletedProducts, IEnumerable<AccountPrice> accountPrices, IEnumerable<DeletedAccountPrice> deletedAccountPrices, string accessKey)
        {
            var processedProducts = new List<ProcessedProduct>();
            var processedAccountPrices = new List<ProcessedAccountPrice>();

            foreach (var product in preProcessProducts)
            {
                ProcessedProduct processedProduct = ConvertProduct(product);
                processedProducts.Add(processedProduct);
            }

            foreach (var price in accountPrices)
            {
                ProcessedAccountPrice processedAccountPrice = ConvertAccountPrice(price);
                processedAccountPrices.Add(processedAccountPrice);
            }

            return ComposeDeltas.GenerateDeltas(processedProducts, deletedProducts, processedAccountPrices, deletedAccountPrices, accessKey);
        }

        public static ProcessedCategoryFeed ProcessCategories(IEnumerable<Category> preProcessCategories)
        {
            return new ProcessedCategoryFeed { Categories = preProcessCategories.ToArray() };
        }

        public static ProcessedUserFeed ProcessUsers(IEnumerable<User> preProcessUsers)
        {
            var processedUsers = preProcessUsers.Select((user) =>
            {
                var processedUser = new ProcessedUser
                {
                    City = user.City,
                    Country = user.Country,
                    DOB = user.DOB,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    LastName = user.LastName,
                    Salutation = user.Salutation,
                    State = user.State,
                    UserId = user.UserId
                };

                var customFields = new Dictionary<string, JToken>();
                foreach (var customField in user.CustomFields)
                {
                    var values = customField.Value.Select((value) => { return WebUtility.HtmlEncode(value); });
                    customFields.Add(WebUtility.HtmlEncode(customField.Key), new JArray(values));
                }

                processedUser.CustomFields = customFields;
                return processedUser;
            });
            return new ProcessedUserFeed { Users = processedUsers.ToArray() };
        }

        private static ProcessedAccountPrice ConvertAccountPrice(AccountPrice accountPrice)
        {
            return new ProcessedAccountPrice
            {
                AccountId = accountPrice.AccountId,
                ParentId = accountPrice.ParentId,
                Prices = accountPrice.Prices?.Select((price) => { return $"{price.Value} {price.Currency}"; }).ToArray(),
                SalePrices = accountPrice.SalePrices?.Select((price) => { return $"{price.Value} {price.Currency}"; }).ToArray(),
                Sku = accountPrice.Sku
            };
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
                AssociatedTitles = new[] { product.Title },
                Visibility = (int)product.Visibility
            };

            var associatedSkus = new List<string> { product.Sku };
            var associatedTitles = new List<string> { product.Title };
            var prices = product.Prices.Count != 0 ? product.Prices : new List<Price>();
            var salePrices = product.SalePrices.Count != 0 ? product.SalePrices : new List<Price>();
            
            var attributes = new Dictionary<string, JToken>();
            foreach (var variant in product.Variants)
            {
                associatedSkus.Add(variant.Sku);
                associatedTitles.Add(variant.Title);              
                prices.AddRange(variant.Prices);
                salePrices.AddRange(variant.SalePrices);

                foreach (var attr in variant.Attributes)
                {
                    var values = attr.Value.Where(val => !string.IsNullOrWhiteSpace(val)).Select((value) => { return WebUtility.HtmlEncode(value); });
                    if(values.Count() > 0){
                        if(attributes.ContainsKey(WebUtility.HtmlEncode(attr.Key)))
                        {
                            var oldValues = attributes[WebUtility.HtmlEncode(attr.Key)].Select(x => x.ToString()).ToList();
                            attributes[WebUtility.HtmlEncode(attr.Key)] = new JArray(oldValues.Concat(values).Distinct());
                        }else{
                            attributes.Add(WebUtility.HtmlEncode(attr.Key), new JArray(values));
                        }
                    }
                }
            }

            processedProduct.AssociatedSkus = associatedSkus.ToArray();
            processedProduct.AssociatedTitles = associatedTitles.ToArray();
            processedProduct.Prices = ProcessPrices(prices);
            processedProduct.SalePrices = ProcessPrices(salePrices);

            foreach (var attr in product.Attributes)
            {                
                var values = attr.Value.Where(val => !string.IsNullOrWhiteSpace(val)).Select((value) => { return WebUtility.HtmlEncode(value); });
                if(values.Count() > 0){
                    if(attributes.ContainsKey(WebUtility.HtmlEncode(attr.Key)))
                    {
                        var oldValues = attributes[WebUtility.HtmlEncode(attr.Key)].Select(x => x.ToString()).ToList();
                        attributes[WebUtility.HtmlEncode(attr.Key)] = new JArray(oldValues.Concat(values).Distinct());
                    }else{
                        attributes.Add(WebUtility.HtmlEncode(attr.Key), new JArray(values));
                    }
                }
            }

            processedProduct.Attributes = attributes;
            return processedProduct;
        }

        private static string[] ProcessPrices(List<Price> prices)
        {
            var minMaxedPrices = prices.GroupBy((price) => price.Currency)
            .Select((currencyGrouping) =>
            {
                var orderedPrices = currencyGrouping.OrderBy((price) => { return price.Value; });
                return new List<Price> { orderedPrices.First(), orderedPrices.Last() };
            })
            .SelectMany((minMaxedCurrencyGroupings) => minMaxedCurrencyGroupings)
            .Distinct();

            var convertedPrices = new List<string>();

            foreach (var price in minMaxedPrices)
            {
                convertedPrices.Add($"{price.Value} {price.Currency}");
            }

            return convertedPrices.Count() > 0 ? convertedPrices.ToArray() : null;
        }
    }
}