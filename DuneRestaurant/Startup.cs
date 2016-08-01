using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DuneRestaurant.Startup))]
namespace DuneRestaurant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
