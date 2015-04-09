namespace SignalR.HubStrong.Tests.TestHelpers
{
    using FakeItEasy;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public static class Async
    {
        public static AsyncCallConfiguration CallTo(Expression<Action> callSpecification)
        {
            return new AsyncCallConfiguration(callSpecification);
        }
    }

    public class AsyncCallConfiguration
    {
        private readonly Expression<Action> callSpecification;

        public AsyncCallConfiguration(Expression<Action> callSpecification)
        {
            this.callSpecification = callSpecification;
        }

        public async Task MustHappen()
        {
            var callTaskCompletionSource = new TaskCompletionSource<object>();
            A.CallTo(callSpecification).Invokes(x => callTaskCompletionSource.SetResult(null));

            var assertionFailed = false;
            try
            {
                A.CallTo(callSpecification).MustHaveHappened();
            }
            catch
            {
                assertionFailed = true;
            }

            if (assertionFailed)
            {
                var completed = callTaskCompletionSource.Task;
                var timeout = Task.Delay(TimeSpan.FromSeconds(30));
                await Task.WhenAny(completed, timeout);
                A.CallTo(callSpecification).MustHaveHappened();
            }
        }
    }
}
