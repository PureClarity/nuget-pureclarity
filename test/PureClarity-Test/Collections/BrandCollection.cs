using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

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
            BrandCollection.AddItem(brand);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check Brand is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddBrandTwice()
        {
            var BrandCollection = GetNewBrandCollection();

            var brand = new Brand("Test");
            BrandCollection.AddItem(brand);
            BrandCollection.AddItem(brand);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Brands get added to collection
        /// </summary>
        [Fact]        
        public void AddBrands()
        {
            var BrandCollection = GetNewBrandCollection();

            var brands = new List<Brand> { new Brand("Test"), new Brand("Test2") };           
            BrandCollection.AddItems(brands);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Brands get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddBrandsTwice()
        {
            var BrandCollection = GetNewBrandCollection();

            var brands = new List<Brand> { new Brand("Test"), new Brand("Test2"), new Brand("Test2") };
            BrandCollection.AddItems(brands);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        #endregion

        #region Remove Brands

        /// <summary>
        /// Check Brand is removed from collection
        /// </summary>
        [Fact]
        public void RemoveBrand()
        {
            var id = "Test";
            var BrandCollection = GetNewBrandCollection();
            
            var brand = new Brand(id);
            BrandCollection.AddItem(brand);
            BrandCollection.RemoveItemFromCollection(id);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check Brands are removed from collection
        /// </summary>
        [Fact]
        public void RemoveBrands()
        {
            var id = "Test";
            var id2 = "Test2";
            var BrandCollection = GetNewBrandCollection();
            
            var brands = new List<Brand> { new Brand(id), new Brand(id2) };
            BrandCollection.AddItems(brands);

            var BrandIds = new List<string> { id, id2 };
            BrandCollection.RemoveItemsFromCollection(BrandIds);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing Brand that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveBrandNotInCollection()
        {
            var sku = "Test";          
            var BrandCollection = GetNewBrandCollection();
            BrandCollection.RemoveItemFromCollection(sku);

            var state = BrandCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
 