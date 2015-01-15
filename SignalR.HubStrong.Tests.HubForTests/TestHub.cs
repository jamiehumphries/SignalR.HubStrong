namespace SignalR.HubStrong.Tests.HubForTests
{
    using Microsoft.AspNet.SignalR;

    public class TestHub : Hub
    {
        public void CallHello()
        {
            Clients.All.Hello();
        }
    }
}
