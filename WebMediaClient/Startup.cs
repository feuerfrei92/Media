using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebMediaClient.Startup))]
namespace WebMediaClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
