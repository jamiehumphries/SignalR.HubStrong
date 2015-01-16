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

    [TestFixture(true)]
    [TestFixture(false)]
    public class ClientMappingExtensionsTests
    {
        private readonly bool useGenericMapping;

        private HubConnection hubConnection;
        private IHubProxy hub;
        private ITestHubClient client;

        public ClientMappingExtensionsTests(bool useGenericMapping)
        {
            this.useGenericMapping = useGenericMapping;
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            hubConnection = new HubConnection(TestHubSite.Url);
            hub = hubConnection.CreateHubProxy("TestHub");

            if (useGenericMapping)
            {
                client = A.Fake<ITestHubClient>();
                hub.MapClientMethods<ITestHubClient>(client);
            }
            else
            {
                client = A.Fake<TestHubClient>();
                hub.MapClientMethods(new TestHubClient(client));
            }

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
            await Task.Delay(200);

            // Then
            A.CallTo(() => client.Hello()).MustHaveHappened();
        }

        [Test]
        [Repeat(3)]
        public async Task Can_map_client_methods_with_any_number_of_arguments()
        {
            // When
            await hub.Invoke("CallArgsMethods", "a", "b", "c", "d", "e", "f", "g");
            await Task.Delay(200);

            // Then
            A.CallTo(() => client.Args0()).MustHaveHappened();
            A.CallTo(() => client.Args1("a")).MustHaveHappened();
            A.CallTo(() => client.Args2("a", "b")).MustHaveHappened();
            A.CallTo(() => client.Args3("a", "b", "c")).MustHaveHappened();
            A.CallTo(() => client.Args4("a", "b", "c", "d")).MustHaveHappened();
            A.CallTo(() => client.Args5("a", "b", "c", "d", "e")).MustHaveHappened();
            A.CallTo(() => client.Args6("a", "b", "c", "d", "e", "f")).MustHaveHappened();
            A.CallTo(() => client.Args7("a", "b", "c", "d", "e", "f", "g")).MustHaveHappened();
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
            await Task.Delay(200);

            // Then
            A.CallTo(() => client.MultipleParamTypes(intParam, longParam, boolParam, stringParam,
                A<string[]>.That.Matches(o => o.Contains("hello") && o.Contains("world")),
                A<Dictionary<string, int>>.That.Matches(d => d["test"].Equals(100)),
                A<Foo>.That.Matches(f => f.Bar == "Baz"))).MustHaveHappened();
        }
    }
}
