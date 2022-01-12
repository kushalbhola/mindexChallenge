using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using challenge.Models;
using code_challenge.Tests.Integration.Extensions;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }
        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetEmployeeReportingStructureByIdSuccess()
        {
            // Arrange
            var EmployeeID = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/{EmployeeID}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(reportingStructure.NumberOfReports, 4);
        }
        [TestMethod]
        public void GetEmployeeReportingStructureByIdFailure()
        {
            // Arrange
            var EmployeeID = "TestEmployeeID";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/{EmployeeID}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.IsTrue(reportingStructure.IsError);
        }
    }
}
