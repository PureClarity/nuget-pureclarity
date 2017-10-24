using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity.Validators
{
    internal class ProductValidator : PCValidationBase
    {
        private HashSet<string> Currencies;

        const string _currencyError = "All products that are not a parent must contain a price for every currency present in the feed";
        const string _salesCurrencyError = "A product must contain a sales price for every currency present in the feed, when a product has a sales price";

        public ProductValidator()
        {
            Currencies = new HashSet<string>();
        }

        public void ValidateUnnassignedVariants(ConcurrentDictionary<string, List<Product>> unnassignedVariants)
        {
            if (unnassignedVariants.Count != 0)
            {
                foreach (var kvp in unnassignedVariants)
                {
                    var errors = new List<string>();
                    foreach (var variant in kvp.Value)
                    {
                        errors.Add($"Item {variant.Id} was added but parent {kvp.Key} was never added");
                    }

                    InvalidRecords.Add(kvp.Key, errors);
                }
            }
        }

        public void GetAllCurrencies(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var productPriceCurrencies = product.Prices.Select((price) => { return price.Currency; });
                if (productPriceCurrencies != null)
                {
                    foreach (var currency in productPriceCurrencies)
                    {
                        if (!Currencies.Contains(currency))
                        {
                            Currencies.Add(currency);
                        }
                    }
                }

                if (product.Variants.Count > 0)
                {
                    foreach (var variant in product.Variants)
                    {
                        var variantPriceCurrencies = variant.Prices.Select((price) => { return price.Currency; });
                        if (variantPriceCurrencies != null)
                        {
                            foreach (var currency in variantPriceCurrencies)
                            {
                                if (!Currencies.Contains(currency))
                                {
                                    Currencies.Add(currency);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ValidateProduct(Product product)
        {
            var errors = new List<string>();
            var priceErrors = ValidatePrices(product);
            errors.AddRange(priceErrors);

            if (string.IsNullOrWhiteSpace(product.ParentId) && product.Variants.Count == 0 && product.Prices.Count == 0)
            {
                errors.Add("Product has no parent, no variants and no price. A product must have a price or must be a parent with variants.");
            }

            if (errors.Count != 0)
            {
                InvalidRecords.Add(product.Id, errors);
            }
        }

        IEnumerable<string> ValidatePrices(Product product)
        {
            var priceErrors = new List<string>();

            if (product.Prices.Count > 0)
            {
                ValidateCurrencies(product.Prices, ref priceErrors, false);
            }

            if (product.SalePrices.Count > 0)
            {
                if (product.Prices.Count == 0)
                {
                    priceErrors.Add($"Product {product.Sku} has no prices but has sale prices. A product must have prices to have sale prices");
                }

                ValidateCurrencies(product.SalePrices, ref priceErrors, true);
            }

            if (product.Variants.Count > 0)
            {
                foreach (var variant in product.Variants)
                {
                    if (variant.Prices.Count > 0)
                    {
                        ValidateCurrencies(variant.Prices, ref priceErrors, false);
                    }

                    if (variant.SalePrices.Count > 0)
                    {
                        if (variant.Prices.Count == 0)
                        {
                            priceErrors.Add($"Variant {variant.Sku} has no prices but has sale prices. A variant must have prices to have sale prices");
                        }

                        ValidateCurrencies(variant.SalePrices, ref priceErrors, true);
                    }
                }
            }


            return priceErrors;
        }

        private void ValidateCurrencies(IEnumerable<Price> prices, ref List<string> errors, bool salePrices)
        {
            var missingCurrencies = Currencies.Where((currency) =>
            {
                return !prices.Any((price) =>
                {
                    return string.Equals(price.Currency, currency, System.StringComparison.OrdinalIgnoreCase);
                });
            });

            foreach (var currency in missingCurrencies)
            {
                errors.Add($"Missing price for currency {currency}. {(salePrices ? _salesCurrencyError : _currencyError)}");
            }
        }
    }
}