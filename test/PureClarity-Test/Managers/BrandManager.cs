using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

namespace PureClarity_Test
{
    public class BrandManagerTest
    {

        private BrandManager GetNewBrandManager()
        {
            return new BrandManager();
        }

        #region Add Brands

        /// <summary>
        /// Check Brand is added to collection
        /// </summary>
        [Fact]        
        public void AddBrand()
        {
            var BrandManager = GetNewBrandManager();

            var Brand = new Brand("Test");
            BrandManager.AddItem(Brand);

            var state = BrandManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check Brand is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddBrandTwice()
        {
            var BrandManager = GetNewBrandManager();

            var Brand = new Brand("Test");
            BrandManager.AddItem(Brand);
            BrandManager.AddItem(Brand);

            var state = BrandManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Brands get added to collection
        /// </summary>
        [Fact]        
        public void AddBrands()
        {
            var BrandManager = GetNewBrandManager();

            var Brands = new List<Brand> { new Brand("Test"), new Brand("Test2") };           
            BrandManager.AddItems(Brands);

            var state = BrandManager.GetManagerState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Brands get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddBrandsTwice()
        {
            var BrandManager = GetNewBrandManager();

            var Brands = new List<Brand> { new Brand("Test"), new Brand("Test2"), new Brand("Test2") };
            BrandManager.AddItems(Brands);

            var state = BrandManager.GetManagerState();
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
            var BrandManager = GetNewBrandManager();
            
            var Brand = new Brand(id);
            BrandManager.AddItem(Brand);
            BrandManager.RemoveItem(id);

            var state = BrandManager.GetManagerState();
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
            var BrandManager = GetNewBrandManager();
            
            var Brands = new List<Brand> { new Brand(id), new Brand(id2) };
            BrandManager.AddItems(Brands);

            var BrandIds = new List<string> { id, id2 };
            BrandManager.RemoveItems(BrandIds);

            var state = BrandManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing Brand that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveBrandNotInCollection()
        {
            var sku = "Test";          
            var BrandManager = GetNewBrandManager();
            BrandManager.RemoveItem(sku);

            var state = BrandManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
 