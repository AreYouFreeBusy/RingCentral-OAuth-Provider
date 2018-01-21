using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RingCentral_OAuth_Demo.Startup))]
namespace RingCentral_OAuth_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
