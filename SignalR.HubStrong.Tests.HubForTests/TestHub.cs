namespace SignalR.HubStrong.Tests.HubForTests
{
    using Microsoft.AspNet.SignalR;
    using SignalR.HubStrong.Tests.Contracts;
    using System.Collections.Generic;

    public class TestHub : Hub<ITestHubClient>, ITestHubServer
    {
        public void CallHello()
        {
            Clients.All.Hello();
        }

        public void CallArgsMethods(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
        {
            Clients.All.Args0();
            Clients.All.Args1(arg1);
            Clients.All.Args2(arg1, arg2);
            Clients.All.Args3(arg1, arg2, arg3);
            Clients.All.Args4(arg1, arg2, arg3, arg4);
            Clients.All.Args5(arg1, arg2, arg3, arg4, arg5);
            Clients.All.Args6(arg1, arg2, arg3, arg4, arg5, arg6);
            Clients.All.Args7(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public void CallMultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam)
        {
            Clients.All.MultipleParamTypes(intParam, longParam, boolParam, stringParam, arrayParam, dictionaryParam, objectParam);
        }
    }
}
