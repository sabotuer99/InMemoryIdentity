using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InMemoryIdentity.Startup))]
namespace InMemoryIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
