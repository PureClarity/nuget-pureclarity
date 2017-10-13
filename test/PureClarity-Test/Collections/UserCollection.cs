using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

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
            UserCollection.AddItem(User);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check User is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddUserTwice()
        {
            var UserCollection = GetNewUserCollection();

            var User = new User("Test");
            UserCollection.AddItem(User);
            UserCollection.AddItem(User);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Users get added to collection
        /// </summary>
        [Fact]        
        public void AddUsers()
        {
            var UserCollection = GetNewUserCollection();

            var Users = new List<User> { new User("Test"), new User("Test2") };           
            UserCollection.AddItems(Users);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Users get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddUsersTwice()
        {
            var UserCollection = GetNewUserCollection();

            var Users = new List<User> { new User("Test"), new User("Test2"), new User("Test2") };
            UserCollection.AddItems(Users);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(2, state.ItemCount);
        }

        #endregion

        #region Remove Users

        /// <summary>
        /// Check User is removed from collection
        /// </summary>
        [Fact]
        public void RemoveUser()
        {
            var id = "Test";
            var UserCollection = GetNewUserCollection();
            
            var User = new User(id);
            UserCollection.AddItem(User);
            UserCollection.RemoveItemFromCollection(id);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check Users are removed from collection
        /// </summary>
        [Fact]
        public void RemoveUsers()
        {
            var id = "Test";
            var id2 = "Test2";
            var UserCollection = GetNewUserCollection();
            
            var Users = new List<User> { new User(id), new User(id2) };
            UserCollection.AddItems(Users);

            var UserIds = new List<string> { id, id2 };
            UserCollection.RemoveItemsFromCollection(UserIds);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing User that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveUserNotInCollection()
        {
            var sku = "Test";          
            var UserCollection = GetNewUserCollection();
            UserCollection.RemoveItemFromCollection(sku);

            var state = UserCollection.GetCollectionState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
