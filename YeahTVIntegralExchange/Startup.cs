using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YeahTVIntegralExchange.Startup))]
namespace YeahTVIntegralExchange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
