using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Collections;
using PureClarity.Models;

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

            var category = new Category("Test", "Test", "Test");
            var result = CategoryCollection.AddItem(category);

            Assert.Equal(true, result.Success);
        }

        /// <summary>
        /// Check Category is added to collection and then returns error
        /// </summary>
        [Fact]
        public void AddCategoryTwice()
        {
            var CategoryCollection = GetNewCategoryCollection();
            var id = "Test";
            var category = new Category(id, "Test", "Test");
            var result = CategoryCollection.AddItem(category);
            result = CategoryCollection.AddItem(category);

            Assert.Equal(false, result.Success);
            Assert.Equal($"Duplicate item found: {id}. Newest item not added.", result.Error);
        }

        /// <summary>
        /// Check multiple Categories get added to collection
        /// </summary>
        [Fact]
        public void AddCategories()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var Categories = new List<Category> { new Category("Test", "Test", "Test"), new Category("Test2", "Test", "Test") };
            var results = CategoryCollection.AddItems(Categories);

            Assert.Equal(2, results.Count());
            Assert.Equal(true, results.All((result) => { return result.Success; }));
        }

        /// <summary>
        /// Check multiple Categories get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddCategoriesTwice()
        {
            var CategoryCollection = GetNewCategoryCollection();

            var id = "Test2";
            var Categories = new List<Category> { new Category("Test", "Test", "Test"), new Category(id, "Test", "Test"), new Category(id, "Test", "Test") };
            var results = CategoryCollection.AddItems(Categories);

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

        #region Remove Categories

        /// <summary>
        /// Check Category is removed from collection
        /// </summary>
        [Fact]
        public void RemoveCategory()
        {
            var sku = "Test";
            var CategoryCollection = GetNewCategoryCollection();

            var Category = new Category(sku, "Test", "Test");
            CategoryCollection.AddItem(Category);
            var result = CategoryCollection.RemoveItemFromCollection(sku);

            Assert.Equal(true, result.Success);
            Assert.Equal(Category, result.Item);
        }

        /// <summary>
        /// Check Categories are removed from collection
        /// </summary>
        [Fact]
        public void RemoveCategories()
        {
            var sku = "Test";
            var sku2 = "Test2";
            var CategoryCollection = GetNewCategoryCollection();

            var prod1 = new Category(sku, "Test", "Test");
            var prod2 = new Category(sku2, "Test", "Test");
            var Categories = new List<Category> { prod1, prod2 };
            CategoryCollection.AddItems(Categories);

            var CategoryIds = new List<string> { sku, sku2 };
            var results = CategoryCollection.RemoveItemsFromCollection(CategoryIds);

            Assert.Equal(2, results.Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(prod1, results.First().Item);
            Assert.Equal(prod2, results.Last().Item);
        }

        /// <summary>
        /// Check removing Category that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveCategoryNotInCollection()
        {
            var sku = "Test";
            var CategoryCollection = GetNewCategoryCollection();
            var result = CategoryCollection.RemoveItemFromCollection(sku);

            Assert.Equal(false, result.Success);
            Assert.Equal($"{sku} could not be removed.", result.Error);
        }

        #endregion
    }
}
