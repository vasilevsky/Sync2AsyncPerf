using System.Data.Entity.Core;
using Domain;
using Domain.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace API.Tests
{
    [TestClass]
    public class CustomerOrdersServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void AddOrder_throws_when_customer_not_found()
        {
            var context = new Mock<ICustomerOrdersContext>();
            context.Setup(m => m.GetCustomer(9)).Returns((Customer)null);
            var service = new CustomerOrdersService(new TestCustomerOrdersContextFactory(context.Object));

            service.AddOrder(9, 777);
            context.VerifyAll();
        }

        [TestMethod]
        public void CreateCustomerTest()
        {
            var context = new Mock<ICustomerOrdersContext>();

            var service = new CustomerOrdersService(new TestCustomerOrdersContextFactory(context.Object));

            var customerId = service.CreateCustomer(new CustomerData()
            {
                Name = "chuck",
                Email = "email"
            });

            context.Verify(m => m.GetCustomers(), Times.Once);
            context.Verify(m => m.AddCustomer(It.IsAny<Customer>()), Times.Once);
            context.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
