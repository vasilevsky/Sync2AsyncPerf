using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Data;

namespace API.Tests
{
    /// <summary>
    /// Implementes factory for in-memory CustomerOrders context.
    /// </summary>
    public class InMemoryCustomerOrderContextFactory : ICustomerOrdersContextFactory
    {
        private readonly ICustomerOrdersContext context;
        public InMemoryCustomerOrderContextFactory(ICustomerOrdersContext context)
        {
            this.context = context;
        }

        public ICustomerOrdersContext CreateContext(IsolationLevel isolationLevel)
        {
            return context;
        }

        ICustomerOrdersContext ICustomerOrdersContextFactory.CreateContext()
        {
            return context;
        }
    }

    /// <summary>
    /// Implements in-memory CustomerOrders context used for test perposes.
    /// </summary>
    public class InMemoryCustomerOrderContext : ICustomerOrdersContext
    {
        private List<Customer> customers = new List<Customer>();

        public List<Customer> Customers
        {
            get { return customers; }
        }

        public void Dispose()
        {
        }

        public Customer GetCustomer(int id)
        {
            return customers.SingleOrDefault(m => m.Id == id);
        }

        public Task<Customer> GetCustomerAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void AddCustomer(Domain.Customer customer)
        {
            customers.Add(customer);
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Customer> GetCustomers()
        {
            return customers.AsQueryable();
        }
    }
}
