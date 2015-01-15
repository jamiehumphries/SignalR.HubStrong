using Microsoft.Owin;
using SignalR.HubStrong.Tests.HubForTests;

[assembly: OwinStartup(typeof(Startup))]

namespace SignalR.HubStrong.Tests.HubForTests
{
    using Microsoft.AspNet.SignalR;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration { EnableDetailedErrors = true };
            app.MapSignalR(config);
        }
    }
}
