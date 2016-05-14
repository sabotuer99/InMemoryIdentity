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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            throw new NotImplementedException();
        }
    }
}