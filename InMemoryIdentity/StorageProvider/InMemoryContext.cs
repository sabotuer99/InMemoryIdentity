using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace InMemoryIdentity.StorageProvider
{
    public class InMemoryContext : IDisposable
    {

        public Dictionary<string, IdentityRole> roles;
        private Dictionary<string, List<Claim>> claims;
        private Dictionary<string, string> userLogins;
        private Dictionary<string, string> userRoles;
        private Dictionary<string, IdentityUser> users;

        public void Dispose()
        {
        }
    }
}