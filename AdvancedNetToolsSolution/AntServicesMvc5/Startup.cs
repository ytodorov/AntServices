using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AntServicesMvc5.Startup))]
namespace AntServicesMvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
