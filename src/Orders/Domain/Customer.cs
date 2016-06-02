using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Customer
    {
        private Customer()
        { }

        private Customer(string name, string email)
        {
            Orders = new Collection<Order>();
            Name = name;
            Email = email;
        }

        public virtual int Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public ICollection<Order> Orders { get; private set; }

        /// <summary>
        /// Adds order of particular price to the customer.
        /// </summary>
        /// <param name="price"></param>
        /// <exception cref="ArgumentException">Thrown on non positive order price</exception>
        /// <returns></returns>
        public Order AddOrder(decimal price)
        {
            var order = Order.Create(price);

            return AddOrder(order);
        }

        public void AddOrder(DraftOrder draftOrder)
        {
            var order = new Order(this, draftOrder);

            AddOrder(order);
        }

        private Order AddOrder(Order order)
        {
            if (Orders == null)
            {
                Orders = new Collection<Order>();
            }

            Orders.Add(order);

            return order;
        }

        /// <summary>
        /// Creates Customer instance. 
        /// </summary>
        /// <param name="existingCustomers">Existing customers.</param>
        /// <param name="name">Customer's name.</param>
        /// <param name="email">Customer's e-mail.</param>
        /// <exception cref="ArgumentException">Thrown on either name or email is not specified or customer with such email exeists.</exception>
        /// <returns>Instance of Customer.</returns>
        public static Customer Create(IQueryable<Customer> existingCustomers, string name, string email)
        {
            if (existingCustomers.Any(m => m.Email == email))
            {
                throw new ConstraintException("Customer already exist");
            }

            return Create(name, email);
        }

        public static async Task<Customer> CreateAsync(IQueryable<Customer> existingCustomers, string name, string email)
        {
            if (await existingCustomers.AnyAsync(m => m.Email == email))
            {
                throw new ConstraintException("Customer already exist");
            }

            return Create(name, email);
        }

        /// <summary>
        /// Creates Customer instance. 
        /// </summary>
        /// <param name="name">Customer's name.</param>
        /// <param name="email">Customer's e-mail.</param>
        /// <exception cref="ArgumentException">Thrown on either name or email is not specified.</exception>
        /// <returns>Instance of Customer.</returns>
        internal static Customer Create(string name, string email)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name not specified");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email not specified");
            }

            return new Customer(name, email);
        }
    }
}
