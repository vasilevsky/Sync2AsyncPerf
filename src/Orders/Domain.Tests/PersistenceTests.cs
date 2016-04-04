using System;
using System.Data.Entity;
using System.Linq;
using Domain.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Tests
{
    [TestClass]
    public class PersistenceTests
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<CustomerOrdersContext>());
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Database.Delete(CustomerOrdersContext.Name);
        }

        [TestMethod]
        public void Customer_with_several_orders_saved_correctly()
        {
            var name = Guid.NewGuid().ToString();
            var email = name + "@example.com";
            var id = 0;
            var start = DateTime.UtcNow;
            using (var context = new CustomerOrdersContext())
            {
                var customer = Customer.Create(
                    name: name,
                    email: email);

                customer.AddOrder(7);
                customer.AddOrder(15);

                context.Customers.Add(customer);

                context.SaveChanges();

                id = customer.Id;
            }

            using (var context = new CustomerOrdersContext())
            {
                var customer = context.Customers
                    .Include(m => m.Orders)
                    .Single(m => m.Name == name);

                Assert.AreEqual(id, customer.Id);
                Assert.IsTrue(customer.Id > 0);
                Assert.AreEqual(email, customer.Email);
                Assert.AreEqual(2, customer.Orders.Count);

                var order1 = customer.Orders.ElementAt(0);
                var order2 = customer.Orders.ElementAt(1);
                Assert.AreEqual(7, order1.Price);
                Assert.IsTrue(start < order1.CreatedDate);
                Assert.IsTrue(order1.CreatedDate < DateTime.UtcNow);
                Assert.AreEqual(15, order2.Price);
                Assert.IsTrue(start < order2.CreatedDate);
                Assert.IsTrue(order2.CreatedDate < DateTime.UtcNow);
            }
        }
    }
}
