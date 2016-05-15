using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    /// <summary>
    /// Class that represents the Users table in the InMemoryContext
    /// </summary>
    public class UserTable<TUser>
        where TUser : IdentityUser
    {
        private InMemoryContext _database;

        /// <summary>
        /// Constructor that takes a InMemoryContext instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(InMemoryContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            if (!_database.users.ContainsKey(userId))
                return null;

            return _database.users[userId].UserName;
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            return _database.users.Where(x => x.Value.UserName == userName).FirstOrDefault().Key;
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            if (!_database.users.ContainsKey(userId))
                return null;

            return _database.users[userId] as TUser;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            var result = _database.users.Where(x => x.Value.UserName == userName).Select(x => x.Value).Cast<TUser>().ToList();
            return result.Count > 0 ? result : null;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            var result = _database.users.Where(x => x.Value.Email == email).Select(x => x.Value).Cast<TUser>().ToList();
            return result.Count > 0 ? result : null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            var user = GetUserById(userId);
            return user == null ? null : user.PasswordHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            var user = GetUserById(userId);
            if (user == null)
                return 0;
            user.PasswordHash = passwordHash;
            return 1;
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            var user = GetUserById(userId);
            return user == null ? null : user.SecurityStamp;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            if (_database.users.ContainsKey(user.Id))
            {
                return Update(user);
                //throw new ArgumentException("Insert Failed: User already exists.");
            }
                

            _database.users.Add(user.Id, user);
            return  1;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            return _database.users.Remove(userId) ? 1 : 0;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            if (!_database.users.ContainsKey(user.Id))
            {
                return Insert(user);
                //throw new ArgumentException("Update Failed: User not found.");
            }

            _database.users[user.Id] = user;
            return 1;
        }
    }
}