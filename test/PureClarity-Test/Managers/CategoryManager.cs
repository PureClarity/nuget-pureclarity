using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

namespace PureClarity_Test
{
    public class CategoryManagerTest
    {

        private CategoryManager GetNewCategoryManager()
        {
            return new CategoryManager();
        }

        #region Add Categories

        /// <summary>
        /// Check Category is added to collection
        /// </summary>
        [Fact]        
        public void AddCategory()
        {
            var CategoryManager = GetNewCategoryManager();

            var Category = new Category("Test");
            CategoryManager.AddItem(Category);

            var state = CategoryManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check Category is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddCategoryTwice()
        {
            var CategoryManager = GetNewCategoryManager();

            var Category = new Category("Test");
            CategoryManager.AddItem(Category);
            CategoryManager.AddItem(Category);

            var state = CategoryManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Categories get added to collection
        /// </summary>
        [Fact]        
        public void AddCategories()
        {
            var CategoryManager = GetNewCategoryManager();

            var Categories = new List<Category> { new Category("Test"), new Category("Test2") };           
            CategoryManager.AddItems(Categories);

            var state = CategoryManager.GetManagerState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Categories get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddCategoriesTwice()
        {
            var CategoryManager = GetNewCategoryManager();

            var Categories = new List<Category> { new Category("Test"), new Category("Test2"), new Category("Test2") };
            CategoryManager.AddItems(Categories);

            var state = CategoryManager.GetManagerState();
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
            var CategoryManager = GetNewCategoryManager();
            
            var Category = new Category(id);
            CategoryManager.AddItem(Category);
            CategoryManager.RemoveItem(id);

            var state = CategoryManager.GetManagerState();
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
            var CategoryManager = GetNewCategoryManager();
            
            var Categories = new List<Category> { new Category(id), new Category(id2) };
            CategoryManager.AddItems(Categories);

            var CategoryIds = new List<string> { id, id2 };
            CategoryManager.RemoveItems(CategoryIds);

            var state = CategoryManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing Category that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveCategoryNotInCollection()
        {
            var sku = "Test";          
            var CategoryManager = GetNewCategoryManager();
            CategoryManager.RemoveItem(sku);

            var state = CategoryManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
