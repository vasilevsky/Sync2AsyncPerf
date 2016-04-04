using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Data
{
    /// <summary>
    /// Defines CustomerOrders database context.
    /// </summary>
    public interface ICustomerOrdersContext : IDisposable
    {
        /// <summary>
        /// Provides query to set of Customers.
        /// </summary>
        /// <returns>Query to set of Customers.</returns>
        IQueryable<Customer> GetCustomers();

        /// <summary>
        /// Gets Customer by id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <returns>Customer instance.</returns>
        Customer GetCustomer(int id);

        Task<Customer> GetCustomerAsync(int id);

        /// <summary>
        /// Adds Customer.
        /// </summary>
        /// <param name="customer">Customer instance.</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Persists changes made in context of work.
        /// </summary>
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
