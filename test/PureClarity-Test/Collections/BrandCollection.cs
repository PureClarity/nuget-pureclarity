/* using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Collections;
using PureClarity.Models;

namespace PureClarity_Test
{
    public class BrandCollectionTest
    {

        private BrandCollection GetNewBrandCollection()
        {
            return new BrandCollection();
        }

        #region Add Brands

        /// <summary>
        /// Check Brand is added to collection
        /// </summary>
        [Fact]
        public void AddBrand()
        {
            var BrandCollection = GetNewBrandCollection();

            var brand = new Brand("Test");
            var result = BrandCollection.AddItem(brand);

            Assert.Equal(true, result.Success);
        }

        /// <summary>
        /// Check Brand is added to collection and then returns error
        /// </summary>
        [Fact]
        public void AddBrandTwice()
        {
            var BrandCollection = GetNewBrandCollection();
            var id = "Test";
            var Brand = new Brand(id);
            var result = BrandCollection.AddItem(Brand);
            result = BrandCollection.AddItem(Brand);

            Assert.Equal(false, result.Success);
            Assert.Equal($"Duplicate item found: {id}. Newest item not added.", result.Error);
        }

        /// <summary>
        /// Check multiple Brands get added to collection
        /// </summary>
        [Fact]
        public void AddBrands()
        {
            var BrandCollection = GetNewBrandCollection();

            var Brands = new List<Brand> { new Brand("Test"), new Brand("Test2") };
            var results = BrandCollection.AddItems(Brands);

            Assert.Equal(2, results.Count());
            Assert.Equal(true, results.All((result) => { return result.Success; }));
        }

        /// <summary>
        /// Check multiple Brands get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddBrandsTwice()
        {
            var BrandCollection = GetNewBrandCollection();

            var id = "Test2";
            var Brands = new List<Brand> { new Brand("Test"), new Brand(id), new Brand(id) };
            var results = BrandCollection.AddItems(Brands);

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

        #region Remove Brands

        /// <summary>
        /// Check Brand is removed from collection
        /// </summary>
        [Fact]
        public void RemoveBrand()
        {
            var sku = "Test";
            var BrandCollection = GetNewBrandCollection();

            var Brand = new Brand(sku);
            BrandCollection.AddItem(Brand);
            var result = BrandCollection.RemoveItemFromCollection(sku);

            Assert.Equal(true, result.Success);
            Assert.Equal(Brand, result.Item);
        }

        /// <summary>
        /// Check Brands are removed from collection
        /// </summary>
        [Fact]
        public void RemoveBrands()
        {
            var sku = "Test";
            var sku2 = "Test2";
            var BrandCollection = GetNewBrandCollection();

            var prod1 = new Brand(sku);
            var prod2 = new Brand(sku2);
            var Brands = new List<Brand> { prod1, prod2 };
            BrandCollection.AddItems(Brands);

            var BrandIds = new List<string> { sku, sku2 };
            var results = BrandCollection.RemoveItemsFromCollection(BrandIds);

            Assert.Equal(2, results.Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(prod1, results.First().Item);
            Assert.Equal(prod2, results.Last().Item);
        }

        /// <summary>
        /// Check removing Brand that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveBrandNotInCollection()
        {
            var sku = "Test";
            var BrandCollection = GetNewBrandCollection();
            var result = BrandCollection.RemoveItemFromCollection(sku);
    
            Assert.Equal(false, result.Success);
            Assert.Equal($"{sku} could not be removed.", result.Error);
        }

        #endregion
    }
}
 */