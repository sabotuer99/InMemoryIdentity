using Microsoft.AspNet.Identity;
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
        public Dictionary<string, List<Claim>> claims;
        public Dictionary<string, List<UserLoginInfo>> userLogins;
        public Dictionary<string, List<string>> userRoles;
        public Dictionary<string, IdentityUser> users;

        public InMemoryContext()
        {
            roles = new Dictionary<string, IdentityRole>();
            claims = new Dictionary<string, List<Claim>>();
            userLogins = new Dictionary<string, List<UserLoginInfo>>();
            userRoles = new Dictionary<string, List<string>>();
            users = new Dictionary<string, IdentityUser>();
        }

        public void Dispose()
        {
        }
    }
}