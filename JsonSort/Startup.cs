using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonSort.Startup))]
namespace JsonSort
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
