namespace SignalR.HubStrong.Invocation
{
    using Microsoft.AspNet.SignalR.Client;

    public static class HubProxyFactoryExtensions
    {
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
