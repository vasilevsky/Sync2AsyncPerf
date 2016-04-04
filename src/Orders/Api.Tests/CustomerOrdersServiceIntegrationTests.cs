using System;
using System.Data.Entity;
using System.Linq;
using API.Data;
using Domain;
using Domain.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Customer = Domain.Customer;

namespace API.Tests
{
    [TestClass]
    public class CustomerOrdersServiceIntegrationTests
    {
        private readonly CustomerOrdersService service = new CustomerOrdersService(new CustomerOrderContextFactory());
        private static Customer chuck;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<CustomerOrdersContext>());

            using (var context = new CustomerOrdersContext())
            {
                var customer = Customer.Create("chuck", "chuck@example.com");
                context.Customers.Add(customer);
                context.SaveChanges();

                chuck = customer;
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Database.Delete(CustomerOrdersContext.Name);
        }

        [TestMethod]
        public void AddOrderTest()
        {
            var start = DateTime.UtcNow;

            var orderId = service.AddOrder(chuck.Id, 7);

            using (var context = new ContextFactory().CreateContext())
            {
                var customer = context.Customers
                    .Include("Orders")
                    .Single(m => m.Id == chuck.Id);

                var order = customer.Orders.Single(m => m.Id == orderId);

                Assert.AreEqual(7, order.Price);
                Assert.IsTrue(start < order.CreatedDate);
                Assert.IsTrue(order.CreatedDate < DateTime.UtcNow);
            }
        }

        [TestMethod]
        public void CreateCustomerTest()
        {
            var data = new CustomerData()
            {
                Email = "bruce@example.com",
                Name= "bruce lee"
            };
            var customerId = service.CreateCustomer(data);

            using (var context = new ContextFactory().CreateContext())
            {
                var customer = context.Customers
                    .Single(m => m.Id == customerId);

                Assert.AreEqual(data.Name, customer.Name);
                Assert.AreEqual(data.Email, customer.Email);
            }
        }
    }
}
