using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeniorDesign.Startup))]
namespace SeniorDesign
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
