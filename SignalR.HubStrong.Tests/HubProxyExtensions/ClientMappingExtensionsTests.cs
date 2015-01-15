namespace SignalR.HubStrong.Tests.HubProxyExtensions
{
    using Microsoft.AspNet.SignalR.Client;
    using NUnit.Framework;
    using SignalR.HubStrong.Tests.TestHelpers;
    using System.Threading.Tasks;

    [TestFixture]
    public class ClientMappingExtensionsTests
    {
        [Test]
        public async Task Testing_set_up()
        {
            // Given
            var hubConnection = new HubConnection(TestHubSite.Url);
            var hub = hubConnection.CreateHubProxy("TestHub");
            var saidHello = false;
            hub.On("Hello", () => saidHello = true);
            await hubConnection.Start();

            // When
            await hub.Invoke("CallHello");

            // Then
            Assert.True(saidHello);
        }
    }
}
