using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MasterDetail.Startup))]
namespace MasterDetail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
