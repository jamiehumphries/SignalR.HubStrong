namespace SignalR.HubStrong.Tests
{
    using IISExpressAutomation;
    using NUnit.Framework;
    using SignalR.HubStrong.Tests.TestHelpers;
    using System.IO;

    [SetUpFixture]
    public class TestSetUp
    {
        private IISExpress iisExpress;

        [SetUp]
        public void SetUp()
        {
            var testDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..");
            var solutionDir = Directory.GetParent(testDirectoryPath);
            var path = Path.Combine(solutionDir.FullName, "SignalR.HubStrong.Tests.HubForTests");
            var parameters = new Parameters { Path = path, Port = TestHubSite.Port };
            iisExpress = new IISExpress(parameters);
        }

        [TearDown]
        public void TearDown()
        {
            // ReSharper disable once EmptyGeneralCatchClause
            try
            {
                iisExpress.Dispose();
            }
            catch {}
        }
    }
}
