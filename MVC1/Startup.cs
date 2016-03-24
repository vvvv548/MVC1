using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC1.Startup))]
namespace MVC1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
