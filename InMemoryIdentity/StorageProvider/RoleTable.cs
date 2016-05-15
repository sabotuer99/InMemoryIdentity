using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    public class RoleTable
    {
        
        private InMemoryContext _database;

        /// <summary>
        /// Constructor that takes a InMemoryContext instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleTable(InMemoryContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            return _database.roles.Remove(roleId) ? 1 : 0;
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(IdentityRole role)
        {
            _database.roles.Add(role.Id, role);
            return 1;
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            if (!_database.roles.ContainsKey(roleId))
                return null;

            return _database.roles[roleId].Name;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            return _database.roles.Where(x => x.Value.Name == roleName).SingleOrDefault().Key;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            if (!_database.roles.ContainsKey(roleId))
                return null;

            return _database.roles[roleId];
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            return _database.roles.Where(x => x.Value.Name == roleName).SingleOrDefault().Value;
        }

        public int Update(IdentityRole role)
        {
            if (!_database.roles.ContainsKey(role.Id))
                throw new ArgumentException(String.Format("Update failed: Role with id {0} not found.", role.Id));


            _database.roles[role.Id] = role;
            return 1;
        }
    }
}