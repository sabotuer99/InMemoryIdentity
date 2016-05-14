using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    public class RoleStore<TRole> : IQueryableRoleStore<TRole>
        where TRole : IdentityRole
    {
        public IQueryable<TRole> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public System.Threading.Tasks.Task CreateAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<TRole> FindByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<TRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}