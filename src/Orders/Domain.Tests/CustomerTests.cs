using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Tests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Customer_with_correct_data()
        {
            string email = "bruce@example.com";
            string name = "bruce lee";

            var customer = Customer.Create(new List<Customer>().AsQueryable(), name, email);

            Assert.AreEqual(name, customer.Name);
            Assert.AreEqual(email, customer.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ConstraintException))]
        public void Throws_when_customer_with_such_email_exists()
        {
            var customers = new List<Customer>()
            {
                Customer.Create("a", "b")
            }.AsQueryable();

            var customer = Customer.Create(customers, "a", "b");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_when_name_not_specified()
        {
            var customer = Customer.Create(new List<Customer>().AsQueryable(), "", "b");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_when_email_not_specified()
        {
            var customer = Customer.Create(new List<Customer>().AsQueryable(), "a", "");
        }
    }
}
