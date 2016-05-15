using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    /// <summary>
    /// Class that represents the UserLogins table in the InMemoryContext
    /// </summary>
    public class UserLoginsTable
    {
        
        private InMemoryContext _database;

        /// <summary>
        /// Constructor that takes a InMemoryContext instance 
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTable(InMemoryContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            var userId = user.Id;
            if (!_database.userLogins.ContainsKey(userId))
                return 0;

            var remove = _database.userLogins[userId]
                .Where(x => x.ProviderKey == login.ProviderKey 
                            && x.LoginProvider == login.LoginProvider)
                .FirstOrDefault();

            return _database.userLogins[userId].Remove(remove) ? 1 : 0;
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            if (!_database.userLogins.ContainsKey(userId))
                return 0;

            var count = _database.userLogins[userId].Count();
            _database.userLogins.Remove(userId);
            return count;

        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            if (!_database.userLogins.ContainsKey(user.Id))
                _database.userLogins[user.Id] = new List<UserLoginInfo>();

            if (_database.userLogins[user.Id].Where(x => x.LoginProvider == login.LoginProvider &&
                x.ProviderKey == login.ProviderKey).Count() > 0)
                throw new ArgumentException("Insert failed: Duplicate login info.");

            _database.userLogins[user.Id].Add(login);

            return 1;
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            return _database.userLogins.Where(x => x.Value.Where(
                y => y.LoginProvider == userLogin.LoginProvider && 
                     y.ProviderKey == userLogin.ProviderKey).Count() == 1).FirstOrDefault().Key;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            if (!_database.userLogins.ContainsKey(userId))
                return null;

            return _database.userLogins[userId];
        }
    }
}