/* using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;
using PureClarity.Collections;

namespace PureClarity_Test
{
    public class ProductCollectionTest
    {

        private ProductCollection GetNewProductCollection()
        {
            return new ProductCollection();
        }

        private Product CreateProduct(string testValue)
        {
            var testCategory = new List<string>{ testValue };
            return new Product(testValue, testValue, testValue, testValue, testValue, testCategory);
        }

        #region Add Products

        /// <summary>
        /// Check product is added to collection
        /// </summary>
        [Fact]
        public void AddProduct()
        {
            var productCollection = GetNewProductCollection();

            var product = CreateProduct("Test");
            var result = productCollection.AddItem(product);

            Assert.Equal(true, result.Success);
        }

        /// <summary>
        /// Check product is added to collection and then returns error
        /// </summary>
        [Fact]
        public void AddProductTwice()
        {
            var productCollection = GetNewProductCollection();
            var id = "Test";
            var product = CreateProduct(id);
            var result = productCollection.AddItem(product);
            result = productCollection.AddItem(product);

            Assert.Equal(false, result.Success);
            Assert.Equal($"Duplicate item found: {id}. Newest item not added.", result.Error);
        }

        /// <summary>
        /// Check multiple products get added to collection
        /// </summary>
        [Fact]
        public void AddProducts()
        {
            var productCollection = GetNewProductCollection();

            var products = new List<Product> { CreateProduct("Test"), CreateProduct("Test2") };
            var results = productCollection.AddItems(products);

            Assert.Equal(2, results.Count());
            Assert.Equal(true, results.All((result) => { return result.Success; }));
        }

        /// <summary>
        /// Check multiple products get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddProductsTwice()
        {
            var productCollection = GetNewProductCollection();

            var id = "Test2";
            var products = new List<Product> { CreateProduct("Test"), CreateProduct(id), CreateProduct(id) };
            var results = productCollection.AddItems(products);

            Assert.Equal(3, results.Count());

            Assert.Equal(2, results.Where((result) =>
            {
                return result.Success;
            }).Count());

            Assert.Equal(1, results.Where((result) =>
            {
                return !result.Success && result.Error == $"Duplicate item found: {id}. Newest item not added.";
            }).Count());
        }

        #endregion

        #region Remove Products

        /// <summary>
        /// Check product is removed from collection
        /// </summary>
        [Fact]
        public void RemoveProduct()
        {
            var sku = "Test";
            var productCollection = GetNewProductCollection();

            var product = CreateProduct(sku);
            productCollection.AddItem(product);
            var result = productCollection.RemoveItemFromCollection(sku);

            Assert.Equal(true, result.Success);
            Assert.Equal(product, result.Item);
        }

        /// <summary>
        /// Check products are removed from collection
        /// </summary>
        [Fact]
        public void RemoveProducts()
        {
            var sku = "Test";
            var sku2 = "Test2";
            var productCollection = GetNewProductCollection();

            var prod1 = CreateProduct(sku);
            var prod2 = CreateProduct(sku2);
            var products = new List<Product> { prod1, prod2 };
            productCollection.AddItems(products);

            var productIds = new List<string> { sku, sku2 };
            var results = productCollection.RemoveItemsFromCollection(productIds);

            Assert.Equal(2, results.Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(prod1, results.First().Item);
            Assert.Equal(prod2, results.Last().Item);
        }

        /// <summary>
        /// Check removing product that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveProductNotInCollection()
        {
            var sku = "Test";
            var productCollection = GetNewProductCollection();
            var result = productCollection.RemoveItemFromCollection(sku);

            Assert.Equal(false, result.Success);
            Assert.Equal($"{sku} could not be removed.", result.Error);
        }

        #endregion

        #region

        /// <summary>
        /// Validates a product
        /// </summary>
        [Fact]
        public void ValidateValidProduct()
        {
            var sku = "Test";
            var productCollection = GetNewProductCollection();
            var product = CreateProduct(sku);
            product.Prices.Add(new Price(10.0m, "GBP"));
            productCollection.AddItem(product);

            var result = productCollection.Validate();

            Assert.Equal(true, result.Success);
        }

        /// <summary>
        /// Validates a product
        /// </summary>
        [Fact]
        public void ValidateInvalidProduct()
        {
            var sku = "Test";
            var productCollection = GetNewProductCollection();
            var product = CreateProduct(sku);
            productCollection.AddItem(product);

            var result = productCollection.Validate();

            Assert.Equal(false, result.Success);
            Assert.Equal(1, result.InvalidRecords.Count());
        }

        /// <summary>
        /// Validates a product
        /// </summary>
        [Fact]
        public void ValidateInvalidProducts()
        {
            var sku1 = "Test";
            var sku2 = "Test2";
            var productCollection = GetNewProductCollection();

            var product1 = CreateProduct(sku1);
            product1.Prices.Add(new Price(10.0m, "GBP"));
            product1.Prices.Add(new Price(12.0m, "USD"));
            productCollection.AddItem(product1);

            var product2 = CreateProduct(sku2);
            product2.Prices.Add(new Price(10.0m, "GBP"));
            productCollection.AddItem(product2);

            var result = productCollection.Validate();

            Assert.Equal(false, result.Success);
            Assert.Equal(1, result.InvalidRecords.Count());
        }

        #endregion

    }
}
 */