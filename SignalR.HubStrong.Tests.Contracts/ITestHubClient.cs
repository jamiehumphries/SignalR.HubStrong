namespace SignalR.HubStrong.Tests.Contracts
{
    using System.Collections.Generic;

    public interface ITestHubClient
    {
        void Hello();
        void NoArgs();
        void OneArg(object arg1);
        void TwoArgs(object arg1, object arg2);
        void ThreeArgs(object arg1, object arg2, object arg3);
        void FourArgs(object arg1, object arg2, object arg3, object arg4);
        void FiveArgs(object arg1, object arg2, object arg3, object arg4, object arg5);
        void SixArgs(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);
        void SevenArgs(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7);
        void MultipleParamTypes(int intParam, long longParam, bool boolParam, string stringParam, string[] arrayParam, Dictionary<string, int> dictionaryParam, Foo objectParam);
    }
}
