using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;
using System.Linq;

namespace PureClarity_Test
{
    public class ProductCollectionTest
    {

        private ProductCollection GetNewProductCollection()
        {
            return new ProductCollection();
        }

        #region Add Products

        /// <summary>
        /// Check product is added to collection
        /// </summary>
        [Fact]
        public void AddProduct()
        {
            var productCollection = GetNewProductCollection();

            var product = new Product("Test");
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
            var product = new Product(id);
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

            var products = new List<Product> { new Product("Test"), new Product("Test2") };
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
            var products = new List<Product> { new Product("Test"), new Product(id), new Product(id) };
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

            var product = new Product(sku);
            productCollection.AddItem(product);
            productCollection.RemoveItemFromCollection(sku);

            var state = productCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
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

            var products = new List<Product> { new Product(sku), new Product(sku2) };
            productCollection.AddItems(products);

            var productIds = new List<string> { sku, sku2 };
            productCollection.RemoveItemsFromCollection(productIds);

            var state = productCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing product that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveProductNotInCollection()
        {
            var sku = "Test";
            var productCollection = GetNewProductCollection();
            productCollection.RemoveItemFromCollection(sku);

            var state = productCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
