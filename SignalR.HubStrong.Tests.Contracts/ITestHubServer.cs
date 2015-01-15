namespace SignalR.HubStrong.Tests.Contracts
{
    using System.Collections.Generic;

    public interface ITestHubServer
    {
        void CallHello();
        void CallArgsMethods(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7);
        void CallMultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam);
    }
}
