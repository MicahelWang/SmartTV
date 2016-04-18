using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YeahAppCentre.Startup))]
namespace YeahAppCentre
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
