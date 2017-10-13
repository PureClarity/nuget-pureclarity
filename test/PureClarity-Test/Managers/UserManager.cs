using System;
using Xunit;
using PureClarity;
using System.Collections.Generic;

namespace PureClarity_Test
{
    public class UserManagerTest
    {

        private UserManager GetNewUserManager()
        {
            return new UserManager();
        }

        #region Add Users

        /// <summary>
        /// Check User is added to collection
        /// </summary>
        [Fact]        
        public void AddUser()
        {
            var UserManager = GetNewUserManager();

            var User = new User("Test");
            UserManager.AddItem(User);

            var state = UserManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check User is added to collection and then overwritten
        /// </summary>
        [Fact]        
        public void AddUserTwice()
        {
            var UserManager = GetNewUserManager();

            var User = new User("Test");
            UserManager.AddItem(User);
            UserManager.AddItem(User);

            var state = UserManager.GetManagerState();
            Assert.Equal(1, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Users get added to collection
        /// </summary>
        [Fact]        
        public void AddUsers()
        {
            var UserManager = GetNewUserManager();

            var Users = new List<User> { new User("Test"), new User("Test2") };           
            UserManager.AddItems(Users);

            var state = UserManager.GetManagerState();
            Assert.Equal(2, state.ItemCount);
        }

        /// <summary>
        /// Check multiple Users get added to collection and then overwritten
        /// </summary>
        [Fact]
        public void AddUsersTwice()
        {
            var UserManager = GetNewUserManager();

            var Users = new List<User> { new User("Test"), new User("Test2"), new User("Test2") };
            UserManager.AddItems(Users);

            var state = UserManager.GetManagerState();
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
            var UserManager = GetNewUserManager();
            
            var User = new User(id);
            UserManager.AddItem(User);
            UserManager.RemoveItem(id);

            var state = UserManager.GetManagerState();
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
            var UserManager = GetNewUserManager();
            
            var Users = new List<User> { new User(id), new User(id2) };
            UserManager.AddItems(Users);

            var UserIds = new List<string> { id, id2 };
            UserManager.RemoveItems(UserIds);

            var state = UserManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        /// <summary>
        /// Check removing User that isn't in collection doesn't error
        /// </summary>
        [Fact]
        public void RemoveUserNotInCollection()
        {
            var sku = "Test";          
            var UserManager = GetNewUserManager();
            UserManager.RemoveItem(sku);

            var state = UserManager.GetManagerState();
            Assert.Equal(0, state.ItemCount);
        }

        #endregion
    }
}
