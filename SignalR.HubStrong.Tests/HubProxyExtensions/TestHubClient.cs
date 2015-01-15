namespace SignalR.HubStrong.Tests.HubProxyExtensions
{
    using System.Collections.Generic;
    using SignalR.HubStrong.Tests.Contracts;

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
        public virtual void NoArgs()
        {
            fakeClient.NoArgs();
        }

        [HubClientMethod]
        public virtual void OneArg(object arg1)
        {
            fakeClient.OneArg(arg1);
        }

        [HubClientMethod]
        public virtual void TwoArgs(object arg1, object arg2)
        {
            fakeClient.TwoArgs(arg1, arg2);
        }

        [HubClientMethod]
        public virtual void ThreeArgs(object arg1, object arg2, object arg3)
        {
            fakeClient.ThreeArgs(arg1, arg2, arg3);
        }

        [HubClientMethod]
        public virtual void FourArgs(object arg1, object arg2, object arg3, object arg4)
        {
            fakeClient.FourArgs(arg1, arg2, arg3, arg4);
        }

        [HubClientMethod]
        public virtual void FiveArgs(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            fakeClient.FiveArgs(arg1, arg2, arg3, arg4, arg5);
        }

        [HubClientMethod]
        public virtual void SixArgs(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
        {
            fakeClient.SixArgs(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        [HubClientMethod]
        public virtual void SevenArgs(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
        {
            fakeClient.SevenArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        [HubClientMethod]
        public virtual void MultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam)
        {
            fakeClient.MultipleParamTypes(intParam, longParam, boolParam, stringParam, arrayParam, dictionaryParam, objectParam);
        }
    }
}