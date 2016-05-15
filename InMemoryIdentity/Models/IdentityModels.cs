using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using InMemoryIdentity.StorageProvider;
using System;
using System.Diagnostics;

namespace InMemoryIdentity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType

            try
            {
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                return userIdentity;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.StackTrace);

                var claims = new UserClaimsTable(new InMemoryContext()).FindByUserId(this.Id).Claims;

                var userIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, null);
                userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Id));
                userIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", DefaultAuthenticationTypes.ApplicationCookie));
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, this.UserName));
                
                return userIdentity;
            }

            //var userIdentity = new UserClaimsTable(new InMemoryContext()).FindByUserId(this.Id);
            
            // Add custom user claims here
            
        }
    }

    public class ApplicationDbContext : InMemoryContext
    {
        public ApplicationDbContext()
            : base()
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}