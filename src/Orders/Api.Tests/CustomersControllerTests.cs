using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Data;
using API.Utils;
using Domain.Data;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Customer = Domain.Customer;

namespace API.Tests
{
    [TestClass]
    public class CustomersControllerTests
    {
        CustomerOrdersEntities readContext = new CustomerOrdersEntities();
        private static WcfTestHost wcfTestHost;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<CustomerOrdersContext>());

            using (var context = new CustomerOrdersContext())
            {
                var customer = Customer.Create("chuck", "chuck@example.com");
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            wcfTestHost = new WcfTestHost();
            wcfTestHost.Start();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Database.Delete(CustomerOrdersContext.Name);
            wcfTestHost.Stop();
        }

        //[TestMethod]
        //public async Task GET_customers()
        //{
        //    using (var server = CreateTestServer())
        //    {
        //        var response = await server.HttpClient.GetAsync("V1/customers");

        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        //        var content = await response.Content.ReadAsStringAsync();
        //        Assert.AreEqual(
        //            expected: "[{\"Id\":4,\"Name\":\"chuck\",\"Email\":\"chuck@example.com\",\"OrdersCount\":2}," +
        //                      "{\"Id\":3,\"Name\":\"john\",\"Email\":\"john@example.com\",\"OrdersCount\":0}]",
        //            actual: content);
        //    }
        //}

        [TestMethod]
        public async Task POST_new_customer()
        {
            using (var server = CreateTestServer())
            {
                var content = new StringContent("{\"name\": \"chuck\", \"email\": \"chuck@example.com\"}");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await server.HttpClient.PostAsync(
                    requestUri: "V1/customers",
                    content: content);

                var id = await response.Content.ReadAsAsync<int>();
                var customerId = RestoreDbId(id);

                Assert.IsTrue(customerId > 0);
                var customer = readContext.Customers
                    .Include("Orders")
                    .Single(m => m.Id == customerId);

                Assert.AreEqual(1, customer.Orders.Count);
            }
        }

        [TestMethod]
        public async Task POST_new_customer_async()
        {
            using (var server = CreateTestServer())
            {
                var content = new StringContent("{\"name\": \"chuck\", \"email\": \"chuck@example.com\"}");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await server.HttpClient.PostAsync(
                    requestUri: "V1/customersasync",
                    content: content);

                var id = await response.Content.ReadAsAsync<int>();
                var customerId = RestoreDbId(id);

                Assert.IsTrue(customerId > 0);
                var customer = readContext.Customers
                    .Include("Orders")
                    .Single(m => m.Id == customerId);

                Assert.AreEqual(1, customer.Orders.Count);
            }
        }

        private TestServer CreateTestServer()
        {
            var testServer = TestServer.Create<Startup>();
            return testServer;
        }

        private int RestoreDbId(int id)
        {
            return id - IdentityContent.IdentityBase;
        }
    }
}
