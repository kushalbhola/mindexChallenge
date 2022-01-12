using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;
using System;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensationrecordSuccess()
        {
            //setup
            List<Employee> employeeList = new List<Employee>();
            var testEmployee = new Employee()
            {
                EmployeeId = "101",
                Department = "Engineering",
                FirstName = "Kushal",
                LastName = "Bhola",
                Position = "Senior Software Engineer",
            };
            employeeList.Add(testEmployee);

            var compensation = new Compensation()
            {
                SalaryID = 101,
                employees = employeeList,
                EffectiveDate = Convert.ToDateTime("06/29/2015"),
                Salary = Convert.ToDecimal(1234567.89)
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            //Assert.IsNotNull(newCompensation.EmployeeID);
            Assert.IsNotNull(newCompensation.employees);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
        }
        [TestMethod]
        public void CreateCompensationrecordFailure()
        {
            //setup
            //sending blank request to trigger failure
            var requestContent = new JsonSerialization().ToJson(new Compensation());

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.IsTrue(compensation.IsError);
        }

        [TestMethod]
        public void GetCompensationByIdSuccess()
        {
            //setup
            var EmployeeID = "101";
            var expectedEffectiveDate = Convert.ToDateTime("06/29/2015");
            var expectedSalary = Convert.ToDecimal(1234567.89);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{EmployeeID}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedEffectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(expectedSalary, compensation.Salary);
        }
        [TestMethod]
        public void GetCompensationByIdFailure()
        {
            //setup
            var EmployeeID = "abc";
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{EmployeeID}");
            var response = getRequestTask.Result;
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.IsTrue(compensation.IsError);
        }
    }
}
