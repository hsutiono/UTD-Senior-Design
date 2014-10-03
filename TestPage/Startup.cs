using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestPage.Startup))]
namespace TestPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
