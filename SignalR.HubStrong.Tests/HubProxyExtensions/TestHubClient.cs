namespace SignalR.HubStrong.Tests.HubProxyExtensions
{
    using SignalR.HubStrong.Tests.Contracts;
    using System.Collections.Generic;

    public class TestHubClient : ITestHubClient
    {
        private readonly ITestHubClient fakeClient;

        public TestHubClient(ITestHubClient fakeClient)
        {
            this.fakeClient = fakeClient;
        }

        [HubClientMethod]
        public virtual void Hello()
        {
            fakeClient.Hello();
        }

        [HubClientMethod]
        public virtual void Args0()
        {
            fakeClient.Args0();
        }

        [HubClientMethod]
        public virtual void Args1(object arg1)
        {
            fakeClient.Args1(arg1);
        }

        [HubClientMethod]
        public virtual void Args2(object arg1, object arg2)
        {
            fakeClient.Args2(arg1, arg2);
        }

        [HubClientMethod]
        public virtual void Args3(object arg1, object arg2, object arg3)
        {
            fakeClient.Args3(arg1, arg2, arg3);
        }

        [HubClientMethod]
        public virtual void Args4(object arg1, object arg2, object arg3, object arg4)
        {
            fakeClient.Args4(arg1, arg2, arg3, arg4);
        }

        [HubClientMethod]
        public virtual void Args5(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            fakeClient.Args5(arg1, arg2, arg3, arg4, arg5);
        }

        [HubClientMethod]
        public virtual void Args6(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
        {
            fakeClient.Args6(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        [HubClientMethod]
        public virtual void Args7(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
        {
            fakeClient.Args7(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        [HubClientMethod]
        public virtual void MultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam)
        {
            fakeClient.MultipleParamTypes(intParam, longParam, boolParam, stringParam, arrayParam, dictionaryParam, objectParam);
        }
    }
}
