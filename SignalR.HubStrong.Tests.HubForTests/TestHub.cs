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

        public void CallArgsMethods(
            object arg1,
            object arg2,
            object arg3,
            object arg4,
            object arg5,
            object arg6,
            object arg7)
        {
            Clients.All.NoArgs();
            Clients.All.OneArg(arg1);
            Clients.All.TwoArgs(arg1, arg2);
            Clients.All.ThreeArgs(arg1, arg2, arg3);
            Clients.All.FourArgs(arg1, arg2, arg3, arg4);
            Clients.All.FiveArgs(arg1, arg2, arg3, arg4, arg5);
            Clients.All.SixArgs(arg1, arg2, arg3, arg4, arg5, arg6);
            Clients.All.SevenArgs(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public void CallMultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam)
        {
            Clients.All.MultipleParamTypes(
                intParam,
                longParam,
                boolParam,
                stringParam,
                arrayParam,
                dictionaryParam,
                objectParam);
        }
    }
}
