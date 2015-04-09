namespace SignalR.HubStrong.Tests.TestHelpers
{
    using FakeItEasy;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public static class WaitFor
    {
        public static WaitForConfiguration CallTo(Expression<Action> callSpecification)
        {
            return new WaitForConfiguration(callSpecification);
        }
    }

    public class WaitForConfiguration
    {
        private readonly Expression<Action> callSpecification;

        public WaitForConfiguration(Expression<Action> callSpecification)
        {
            this.callSpecification = callSpecification;
        }

        public void ToHaveHappened()
        {
            var callTaskCompletionSource = new TaskCompletionSource<object>();
            A.CallTo(callSpecification).Invokes(x => callTaskCompletionSource.SetResult(null));
            try
            {
                A.CallTo(callSpecification).MustHaveHappened();
            }
            catch (Exception)
            {
                if (!callTaskCompletionSource.Task.Wait(TimeSpan.FromMinutes(1)))
                {
                    throw;
                }
            }
        }
    }
}
