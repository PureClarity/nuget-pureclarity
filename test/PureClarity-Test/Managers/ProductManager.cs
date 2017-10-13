using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

namespace PureClarity_Test
{
    public class ProductManagerTest
    {

        private ProductManager GetNewProductManager()
        {
            return new ProductManager();
        }

        #region Add Products

        /// <summary>
        /// Check product is added to collection
        /// </summary>
        [Fact]        
        public void AddProduct()
        {
            var productManager = GetNewProductManager();

            var product = new Product("Test");
            productManager.AddItem(product);

            var state = productManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check product is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddProductTwice()
        {
            var productManager = GetNewProductManager();

            var product = new Product("Test");
            productManager.AddItem(product);
            productManager.AddItem(product);

            var state = productManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple products get added to collection
        /// </summary>
        [Fact]        
        public void AddProducts()
        {
            var productManager = GetNewProductManager();

            var products = new List<Product> { new Product("Test"), new Product("Test2") };           
            productManager.AddItems(products);

            var state = productManager.GetManagerState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple products get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddProductsTwice()
        {
            var productManager = GetNewProductManager();

            var products = new List<Product> { new Product("Test"), new Product("Test2"), new Product("Test2") };
            productManager.AddItems(products);

            var state = productManager.GetManagerState();
            Assert.Equal(2, state.ItemCount);
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
            var productManager = GetNewProductManager();
            
            var product = new Product(sku);
            productManager.AddItem(product);
            productManager.RemoveItem(sku);

            var state = productManager.GetManagerState();
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
            var productManager = GetNewProductManager();
            
            var products = new List<Product> { new Product(sku), new Product(sku2) };
            productManager.AddItems(products);

            var productIds = new List<string> { sku, sku2 };
            productManager.RemoveItems(productIds);

            var state = productManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing product that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveProductNotInCollection()
        {
            var sku = "Test";          
            var productManager = GetNewProductManager();
            productManager.RemoveItem(sku);

            var state = productManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
