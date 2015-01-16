namespace SignalR.HubStrong.Invocation
{
    using Microsoft.AspNet.SignalR.Client;

    public static class HubProxyFactoryExtensions
    {
        public static IHubProxy<T> CreateHubProxy<T>(this HubConnection hubConnection) where T : class
        {
            // Assume interface IFooHub is implemented by FooHub.
            var hubName = typeof(T).Name.Substring(1);
            return hubConnection.CreateHubProxy<T>(hubName);
        }

        public static IHubProxy<T> CreateHubProxy<T>(this HubConnection hubConnection, string hubName) where T : class
        {
            return hubConnection.CreateHubProxy(hubName).AsProxyFor<T>();
        }

        public static IHubProxy<T> AsProxyFor<T>(this IHubProxy hubProxy) where T : class
        {
            return new HubProxy<T>(hubProxy);
        }
    }
}
