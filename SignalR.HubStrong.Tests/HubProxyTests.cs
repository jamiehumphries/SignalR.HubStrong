namespace SignalR.HubStrong.Tests
{
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNet.SignalR.Client;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using SignalR.HubStrong.Tests.Contracts;
    using System;

    public interface IProgressTracker
    {
        void OnProgress(int progress);
    }

    [TestFixture]
    public class HubProxyTests
    {
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
