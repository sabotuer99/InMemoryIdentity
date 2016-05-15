using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    /// <summary>
    /// Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTable
    {
        
        private InMemoryContext _database;

        /// <summary>
        /// Constructor that takes a InMemoryContext instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(InMemoryContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            if (!_database.userRoles.ContainsKey(userId))
                return null;

            return _database.userRoles[userId];
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            if (!_database.userRoles.ContainsKey(userId))
                return 0;

            var count = _database.userRoles[userId].Count();
            _database.userRoles.Remove(userId);
            return count;
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            if (!_database.userRoles.ContainsKey(user.Id))
                _database.userRoles[user.Id] = new List<string>();

            if(_database.userRoles[user.Id].Contains(roleId))
                throw new ArgumentException(String.Format("Insert failed: User already assigned to roleId {0}.", roleId));

            _database.userRoles[user.Id].Add(roleId);

            return 1;
        }
    }
}