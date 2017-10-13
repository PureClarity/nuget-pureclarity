using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

namespace PureClarity_Test
{
    public class CategoryCollectionTest
    {

        private CategoryCollection GetNewCategoryCollection()
        {
            return new CategoryCollection();
        }

        #region Add Categories

        /// <summary>
        /// Check Category is added to collection
        /// </summary>
        [Fact]        
        public void AddCategory()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var Category = new Category("Test");
            CategoryCollection.AddItem(Category);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check Category is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddCategoryTwice()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var Category = new Category("Test");
            CategoryCollection.AddItem(Category);
            CategoryCollection.AddItem(Category);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Categories get added to collection
        /// </summary>
        [Fact]        
        public void AddCategories()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var Categories = new List<Category> { new Category("Test"), new Category("Test2") };           
            CategoryCollection.AddItems(Categories);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Categories get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddCategoriesTwice()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var Categories = new List<Category> { new Category("Test"), new Category("Test2"), new Category("Test2") };
            CategoryCollection.AddItems(Categories);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        #endregion

        #region Remove Categories

        /// <summary>
        /// Check Category is removed from collection
        /// </summary>
        [Fact]
        public void RemoveCategory()
        {
            var id = "Test";
            var CategoryCollection = GetNewCategoryCollection();
            
            var Category = new Category(id);
            CategoryCollection.AddItem(Category);
            CategoryCollection.RemoveItemFromCollection(id);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check Categories are removed from collection
        /// </summary>
        [Fact]
        public void RemoveCategories()
        {
            var id = "Test";
            var id2 = "Test2";
            var CategoryCollection = GetNewCategoryCollection();
            
            var Categories = new List<Category> { new Category(id), new Category(id2) };
            CategoryCollection.AddItems(Categories);

            var CategoryIds = new List<string> { id, id2 };
            CategoryCollection.RemoveItemsFromCollection(CategoryIds);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing Category that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveCategoryNotInCollection()
        {
            var sku = "Test";          
            var CategoryCollection = GetNewCategoryCollection();
            CategoryCollection.RemoveItemFromCollection(sku);

            var state = CategoryCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
