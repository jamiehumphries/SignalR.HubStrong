namespace SignalR.HubStrong.Tests.Invocation
{
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNet.SignalR.Client;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using SignalR.HubStrong.Invocation;
    using SignalR.HubStrong.Tests.Contracts;
    using SignalR.HubStrong.Tests.Mapping;
    using SignalR.HubStrong.Tests.TestHelpers;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class HubProxyTests
    {
        private HubConnection hubConnection;
        private IHubProxy<ITestHub> hub;
        private ITestHubClient client;
        private IProgressTracker progressTracker;

        public interface IProgressTracker
        {
            void OnProgress(int progress);
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            hubConnection = new HubConnection(TestHubSite.Url);
            hub = hubConnection.CreateHubProxy<ITestHub>();
            hubConnection.Start().Wait();
        }

        [SetUp]
        public void SetUp()
        {
            client = A.Fake<ITestHubClient>();
            progressTracker = A.Fake<IProgressTracker>();
            hub.On<string>("Foo", x => client.Foo(x));
        }

        [Test]
        public async Task Can_invoke_hub_method_with_no_return_value()
        {
            // When
            await hub.Invoke(h => h.DoFoo("bar"));

            // Then
            WaitFor.CallTo(() => client.Foo("bar")).ToHaveHappened();
        }

        [Test]
        public async Task Can_invoke_hub_method_with_a_return_value()
        {
            // When
            var result = await hub.Invoke(h => h.DoFooAndReturnValue("bar"));

            // Then
            WaitFor.CallTo(() => client.Foo("bar")).ToHaveHappened();
            result.Should().Be("bar");
        }

        [Test]
        public async Task Can_invoke_hub_method_with_progress_tracking_and_no_return_value()
        {
            // When
            await hub.Invoke((h, p) => h.DoFooAndReportProgress("bar", p), (int x) => progressTracker.OnProgress(x));

            // Then
            WaitFor.CallTo(() => progressTracker.OnProgress(100)).ToHaveHappened();
            WaitFor.CallTo(() => client.Foo("bar")).ToHaveHappened();
        }

        [Test]
        public async Task Can_invoke_hub_method_with_progress_tracking_and_a_return_value()
        {
            // When
            var result = await hub.Invoke((h, p) => h.DoFooAndReportProgressAndReturnValue("bar", p), (int x) => progressTracker.OnProgress(x));

            // Then
            WaitFor.CallTo(() => progressTracker.OnProgress(100)).ToHaveHappened();
            WaitFor.CallTo(() => client.Foo("bar")).ToHaveHappened();
            result.Should().Be("bar");
        }

        [TestFixture]
        public class WrappedMembers
        {
            private IHubProxy<ITestHub> hub;
            private IHubProxy wrappedHub;

            [SetUp]
            public void SetUp()
            {
                wrappedHub = A.Fake<IHubProxy>();
                hub = new HubProxy<ITestHub>(wrappedHub);
            }

            [Test]
            public void JsonSerializer_wraps_base_proxy()
            {
                hub.JsonSerializer.Should().Be(wrappedHub.JsonSerializer);
            }

            [Test]
            public void JToken_indexing_wraps_base_proxy_get()
            {
                // Given
                var token = JToken.FromObject(A.Dummy<object>());

                // When
                wrappedHub["token"] = token;

                // Then
                hub["token"].Should().BeEquivalentTo(token);
            }

            [Test]
            public void JToken_indexing_wraps_base_proxy_set()
            {
                // Given
                var token = JToken.FromObject(A.Dummy<object>());

                // When
                hub["token"] = token;

                // Then
                wrappedHub["token"].Should().BeEquivalentTo(token);
            }

            [Test]
            public void Invoke_with_no_return_value_wraps_base_proxy()
            {
                // When
                hub.Invoke("Foo", 42);

                // Then
                A.CallTo(() => wrappedHub.Invoke("Foo", 42)).MustHaveHappened();
            }

            [Test]
            public void Invoke_with_return_value_wraps_base_proxy()
            {
                // When
                hub.Invoke<object>("Foo", 42);

                // Then
                A.CallTo(() => wrappedHub.Invoke<object>("Foo", 42)).MustHaveHappened();
            }

            [Test]
            public void Invoke_with_progress_tracker_and_no_return_value_wraps_base_proxy()
            {
                // Given
                Action<int> onProgress = o => { };

                // When
                hub.Invoke("Foo", onProgress, 42);

                // Then
                A.CallTo(() => wrappedHub.Invoke("Foo", onProgress, 42)).MustHaveHappened();
            }

            [Test]
            public void Invoke_with_progress_tracker_and_return_value_wraps_base_proxy()
            {
                // Given
                Action<int> onProgress = o => { };

                // When
                hub.Invoke<object, int>("Foo", onProgress, 42);

                // Then
                A.CallTo(() => wrappedHub.Invoke<object, int>("Foo", onProgress, 42)).MustHaveHappened();
            }
        }
    }
}
