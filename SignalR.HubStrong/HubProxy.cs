namespace SignalR.HubStrong
{
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

    public interface IHubProxy<T> : IHubProxy where T : class {}

    public class HubProxy<THub> : IHubProxy<THub> where THub : class
    {
        private readonly IHubProxy hubProxy;

        public HubProxy(IHubProxy hubProxy)
        {
            this.hubProxy = hubProxy;
        }

        public JsonSerializer JsonSerializer
        {
            get { return hubProxy.JsonSerializer; }
        }

        public JToken this[string name]
        {
            get { return hubProxy[name]; }
            set { hubProxy[name] = value; }
        }

        public Task Invoke(string method, params object[] args)
        {
            return hubProxy.Invoke(method, args);
        }

        public Task<T> Invoke<T>(string method, params object[] args)
        {
            return hubProxy.Invoke<T>(method, args);
        }

        public Task Invoke<T>(string method, Action<T> onProgress, params object[] args)
        {
            return hubProxy.Invoke(method, onProgress, args);
        }

        public Task<TResult> Invoke<TResult, TProgress>(string method, Action<TProgress> onProgress, params object[] args)
        {
            return hubProxy.Invoke<TResult, TProgress>(method, onProgress, args);
        }

        public Subscription Subscribe(string eventName)
        {
            return hubProxy.Subscribe(eventName);
        }
    }
}
