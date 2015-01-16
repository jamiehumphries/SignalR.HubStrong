namespace SignalR.HubStrong.Invocation
{
    using Castle.DynamicProxy;
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IHubProxy<out T> : IHubProxy where T : class
    {
        Task Invoke(Action<T> method);
        Task Invoke<TProgress>(Action<T, IProgress<TProgress>> method, Action<TProgress> onProgress);
        Task<TResult> Invoke<TResult>(Func<T, TResult> method);
        Task<TResult> Invoke<TResult, TProgress>(Func<T, IProgress<TProgress>, TResult> method, Action<TProgress> onProgress);
    }

    public class HubProxy<THub> : IHubProxy<THub> where THub : class
    {
        private readonly IHubProxy hubProxy;
        private readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public HubProxy(IHubProxy hubProxy)
        {
            this.hubProxy = hubProxy;
        }

        private interface IDummyProgress {}

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

        public Task Invoke(Action<THub> method)
        {
            return Invoke<object>((h, p) => method(h), null);
        }

        public Task Invoke<TProgress>(Action<THub, IProgress<TProgress>> method, Action<TProgress> onProgress)
        {
            return Invoke((h, p) =>
            {
                method(h, p);
                return (object)null;
            }, onProgress);
        }

        public Task<TResult> Invoke<TResult>(Func<THub, TResult> method)
        {
            return Invoke<TResult, object>((h, p) => method(h), null);
        }

        public Task<TResult> Invoke<TResult, TProgress>(Func<THub, IProgress<TProgress>, TResult> method, Action<TProgress> onProgress)
        {
            var interceptor = new HubProxyInterceptor<TResult, TProgress>(hubProxy, onProgress);
            var hubProxyProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<THub>(interceptor);
            method.Invoke(hubProxyProxy, new DummyProgress<TProgress>());
            return interceptor.Task;
        }

        private class HubProxyInterceptor<TResult, TProgress> : IInterceptor
        {
            private readonly IHubProxy hubProxy;
            private readonly Action<TProgress> onProgress;

            internal HubProxyInterceptor(IHubProxy hubProxy, Action<TProgress> onProgress)
            {
                this.hubProxy = hubProxy;
                this.onProgress = onProgress;
            }

            internal Task<TResult> Task { get; private set; }

            public void Intercept(IInvocation invocation)
            {
                var method = invocation.Method.Name;
                if (onProgress == null)
                {
                    Task = hubProxy.Invoke<TResult>(method, invocation.Arguments);
                }
                else
                {
                    var arguments = invocation.Arguments.Where(arg => !(arg is IDummyProgress)).ToArray();
                    Task = hubProxy.Invoke<TResult, TProgress>(method, onProgress, arguments);
                }
            }
        }

        private class DummyProgress<T> : Progress<T>, IDummyProgress {}
    }
}
