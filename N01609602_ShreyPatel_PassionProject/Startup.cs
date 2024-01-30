using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(N01609602_ShreyPatel_PassionProject.Startup))]
namespace N01609602_ShreyPatel_PassionProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
