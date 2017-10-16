using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;
using System.Linq;

namespace PureClarity_Test
{
    public class UserCollectionTest
    {

        private UserCollection GetNewUserCollection()
        {
            return new UserCollection();
        }

        #region Add Users

        /// <summary>
        /// Check User is added to collection
        /// </summary>
        [Fact]
        public void AddUser()
        {
            var UserCollection = GetNewUserCollection();

            var User = new User("Test");
            var result = UserCollection.AddItem(User);

            Assert.Equal(true, result.Success);
        }

        /// <summary>
        /// Check User is added to collection and then returns error
        /// </summary>
        [Fact]
        public void AddUserTwice()
        {
            var UserCollection = GetNewUserCollection();
            var id = "Test";
            var User = new User(id);
            var result = UserCollection.AddItem(User);
            result = UserCollection.AddItem(User);

            Assert.Equal(false, result.Success);
            Assert.Equal($"Duplicate item found: {id}. Newest item not added.", result.Error);
        }

        /// <summary>
        /// Check multiple Users get added to collection
        /// </summary>
        [Fact]
        public void AddUsers()
        {
            var UserCollection = GetNewUserCollection();

            var Users = new List<User> { new User("Test"), new User("Test2") };
            var results = UserCollection.AddItems(Users);

            Assert.Equal(2, results.Count());
            Assert.Equal(true, results.All((result) => { return result.Success; }));
        }

        /// <summary>
        /// Check multiple Users get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddUsersTwice()
        {
            var UserCollection = GetNewUserCollection();

            var id = "Test2";
            var Users = new List<User> { new User("Test"), new User(id), new User(id) };
            var results = UserCollection.AddItems(Users);

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

        #region Remove Users

        /// <summary>
        /// Check User is removed from collection
        /// </summary>
        [Fact]
        public void RemoveUser()
        {
            var sku = "Test";
            var UserCollection = GetNewUserCollection();

            var User = new User(sku);
            UserCollection.AddItem(User);
            var result = UserCollection.RemoveItemFromCollection(sku);

            Assert.Equal(true, result.Success);
            Assert.Equal(User, result.Item);
        }

        /// <summary>
        /// Check Users are removed from collection
        /// </summary>
        [Fact]
        public void RemoveUsers()
        {
            var sku = "Test";
            var sku2 = "Test2";
            var UserCollection = GetNewUserCollection();

            var prod1 = new User(sku);
            var prod2 = new User(sku2);
            var Users = new List<User> { prod1, prod2 };
            UserCollection.AddItems(Users);

            var UserIds = new List<string> { sku, sku2 };
            var results = UserCollection.RemoveItemsFromCollection(UserIds);

            Assert.Equal(2, results.Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(2, results.Where((result) => { return result.Success; }).Count());
            Assert.Equal(prod1, results.First().Item);
            Assert.Equal(prod2, results.Last().Item);
        }

        /// <summary>
        /// Check removing User that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveUserNotInCollection()
        {
            var sku = "Test";
            var UserCollection = GetNewUserCollection();
            var result = UserCollection.RemoveItemFromCollection(sku);
    
            Assert.Equal(false, result.Success);
            Assert.Equal($"{sku} could not be removed.", result.Error);
        }

        #endregion
    }
}
