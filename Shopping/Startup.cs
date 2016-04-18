using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopping.Startup))]
namespace Shopping
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
