using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trial.Startup))]
namespace Trial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
