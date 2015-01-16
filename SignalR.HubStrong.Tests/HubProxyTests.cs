namespace SignalR.HubStrong.Tests
{
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNet.SignalR.Client;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using SignalR.HubStrong.Tests.Contracts;
    using SignalR.HubStrong.Tests.TestHelpers;
    using System;
    using System.Collections;
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
            hub = new HubProxy<ITestHub>(hubConnection.CreateHubProxy("TestHub"));
            client = A.Fake<ITestHubClient>();
            hub.On<string>("Foo", x => client.Foo(x));
            hubConnection.Start().Wait();
        }

        [SetUp]
        public void SetUp()
        {
            progressTracker = A.Fake<IProgressTracker>();

            // Clear recorded calls to client without having to start new hub connection.
            ((IList)Fake.GetFakeManager(client).RecordedCallsInScope).Clear();
        }

        [Test]
        public async Task Can_invoke_hub_method_with_no_return_value()
        {
            // When
            await hub.Invoke(h => h.DoFoo("test1"));
            await Task.Delay(200);

            // Then
            A.CallTo(() => client.Foo("test1")).MustHaveHappened();
        }

        [Test]
        public async Task Can_invoke_hub_method_with_a_return_value()
        {
            // When
            var result = await hub.Invoke(h => h.DoFooAndReturnValue("test2"));
            await Task.Delay(200);

            // Then
            A.CallTo(() => client.Foo("test2")).MustHaveHappened();
            result.Should().Be("test2");
        }

        [Test]
        public async Task Can_invoke_hub_method_with_progress_tracking_and_no_return_value()
        {
            // When
            await hub.Invoke((h, p) => h.DoFooAndReportProgress("test3", p), (int x) => progressTracker.OnProgress(x));
            await Task.Delay(200);

            // Then
            A.CallTo(() => progressTracker.OnProgress(100)).MustHaveHappened();
            A.CallTo(() => client.Foo("test3")).MustHaveHappened();
        }

        [Test]
        public async Task Can_invoke_hub_method_with_progress_tracking_and_a_return_value()
        {
            // When
            var result = await hub.Invoke((h, p) => h.DoFooAndReportProgressAndReturnValue("test4", p), (int x) => progressTracker.OnProgress(x));
            await Task.Delay(200);

            // Then
            A.CallTo(() => progressTracker.OnProgress(100)).MustHaveHappened();
            A.CallTo(() => client.Foo("test4")).MustHaveHappened();
            result.Should().Be("test4");
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
