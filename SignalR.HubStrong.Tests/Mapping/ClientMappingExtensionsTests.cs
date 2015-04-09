namespace SignalR.HubStrong.Tests.Mapping
{
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNet.SignalR.Client;
    using NUnit.Framework;
    using SignalR.HubStrong.Mapping;
    using SignalR.HubStrong.Tests.Contracts;
    using SignalR.HubStrong.Tests.TestHelpers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ClientMappingExtensionsTests
    {
        private HubConnection hubConnection;
        private IHubProxy hub;
        private ITestHubClient client;

        [TestFixtureSetUp]
        public void SetUp()
        {
            hubConnection = new HubConnection(TestHubSite.Url);
            hub = hubConnection.CreateHubProxy("TestHub");
            client = A.Fake<ITestHubClient>();
            hub.MapClientMethods<ITestHubClient>(client);
            hubConnection.Start().Wait();
        }

        [Test]
        public void Can_only_map_interfaces_using_generic_method()
        {
            // When
            Action mappingNonInterface = () => hub.MapClientMethods<object>(client);

            // Then
            mappingNonInterface.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Client_must_implement_interface_used_in_generic_mapping()
        {
            // When
            Action mappingNonImplemetedInterface = () => hub.MapClientMethods<IEnumerable>(client);

            // Then
            mappingNonImplemetedInterface.ShouldThrow<ArgumentException>();
        }

        [Test]
        public async Task Can_map_client_methods()
        {
            // When
            await hub.Invoke("CallHello");

            // Then
            WaitFor.CallTo(() => client.Hello()).ToHaveHappened();
        }

        [Test]
        public async Task Can_map_client_methods_with_any_number_of_arguments()
        {
            // When
            await hub.Invoke("CallArgsMethods", "a", "b", "c", "d", "e", "f", "g");

            // Then
            WaitFor.CallTo(() => client.Args0()).ToHaveHappened();
            WaitFor.CallTo(() => client.Args1("a")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args2("a", "b")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args3("a", "b", "c")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args4("a", "b", "c", "d")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args5("a", "b", "c", "d", "e")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args6("a", "b", "c", "d", "e", "f")).ToHaveHappened();
            WaitFor.CallTo(() => client.Args7("a", "b", "c", "d", "e", "f", "g")).ToHaveHappened();
        }

        [Test]
        public async Task Can_map_methods_with_different_types_of_parameters()
        {
            // Given
            const int intParam = 42;
            const long longParam = 10000000000L;
            const bool boolParam = true;
            const string stringParam = "banana";
            var arrayParam = new[] { "hello", "world" };
            var dictionaryParam = new Dictionary<string, int> { { "test", 100 } };
            var objectParam = new Foo { Bar = "Baz" };

            // When
            await hub.Invoke("CallMultipleParamTypes", intParam, longParam, boolParam, stringParam, arrayParam, dictionaryParam, objectParam);

            // Then
            WaitFor.CallTo(() => client.MultipleParamTypes(intParam, longParam, boolParam, stringParam,
                A<string[]>.That.Matches(o => o.Contains("hello") && o.Contains("world")),
                A<Dictionary<string, int>>.That.Matches(d => d["test"].Equals(100)),
                A<Foo>.That.Matches(f => f.Bar == "Baz"))).ToHaveHappened();
        }
    }
}
