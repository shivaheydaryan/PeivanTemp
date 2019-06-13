using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Peivandyar_Site.Startup))]
namespace Peivandyar_Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
