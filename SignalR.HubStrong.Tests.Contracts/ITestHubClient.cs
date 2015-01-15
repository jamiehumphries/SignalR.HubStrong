namespace SignalR.HubStrong.Tests.Contracts
{
    using System.Collections.Generic;

    public interface ITestHubClient
    {
        void Hello();
        void Args0();
        void Args1(object arg1);
        void Args2(object arg1, object arg2);
        void Args3(object arg1, object arg2, object arg3);
        void Args4(object arg1, object arg2, object arg3, object arg4);
        void Args5(object arg1, object arg2, object arg3, object arg4, object arg5);
        void Args6(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);
        void Args7(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7);
        void MultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam);
    }
}
