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
        //public static InMemoryContext singleton;
        public static bool isInitialized;

        private static Dictionary<string, IdentityRole> _roles;
        private static Dictionary<string, List<Claim>> _claims;
        private static Dictionary<string, List<UserLoginInfo>> _userLogins;
        private static Dictionary<string, List<string>> _userRoles;
        private static Dictionary<string, IdentityUser> _users;

        public Dictionary<string, IdentityRole> roles { get { return _roles; } set { _roles = value; } }
        public Dictionary<string, List<Claim>> claims { get { return _claims; } set { _claims = value; } }
        public Dictionary<string, List<UserLoginInfo>> userLogins { get { return _userLogins; } set { _userLogins = value; } }
        public Dictionary<string, List<string>> userRoles { get { return _userRoles; } set { _userRoles = value; } }
        public Dictionary<string, IdentityUser> users { get { return _users; } set { _users = value; } }

        public InMemoryContext()
        {
            if (!InMemoryContext.isInitialized)
                Init();
        }

        public static void Init(){
            _roles = new Dictionary<string, IdentityRole>();
            _claims = new Dictionary<string, List<Claim>>();
            _userLogins = new Dictionary<string, List<UserLoginInfo>>();
            _userRoles = new Dictionary<string, List<string>>();
            _users = new Dictionary<string, IdentityUser>();
            InMemoryContext.isInitialized = true;
        }

        public void Dispose()
        {
        }
    }
}