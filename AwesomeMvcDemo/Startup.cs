using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AwesomeMvcDemo.Startup))]
namespace AwesomeMvcDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}