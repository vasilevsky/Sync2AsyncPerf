using System.Data.Entity.Core;
using System.Threading.Tasks;
using Domain;
using Domain.Data;

namespace API
{
    public class CustomerOrdersService : ICustomerOrdersService
    {
        private readonly ICustomerOrdersContextFactory contextFactory;

        public CustomerOrdersService(ICustomerOrdersContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        /// <summary>
        /// Adds order to customer.
        /// </summary>
        /// <param name="customerId">Id of customer.</param>
        /// <param name="price">Order price.</param>
        /// <exception cref="ObjectNotFoundException">Throws when customer was not found.</exception>
        public int AddOrder(int customerId, decimal price)
        {
            using (var context = contextFactory.CreateContext())
            {
                var customer = context.GetCustomer(customerId);
                if (customer == null)
                {
                    throw new ObjectNotFoundException(string.Format("Customer with id {0} does not exist", customerId));
                }

                var order = customer.AddOrder(price);

                context.SaveChanges();

                return order.Id;
            }
        }

        public async Task<int> AddOrderAsync(int customerId, decimal price)
        {
            using (var context = contextFactory.CreateContext())
            {
                var customer = await context.GetCustomerAsync(customerId);
                if (customer == null)
                {
                    throw new ObjectNotFoundException(string.Format("Customer with id {0} does not exist", customerId));
                }

                var order = customer.AddOrder(price);

                await context.SaveChangesAsync();

                return order.Id;
            }
        }

        /// <summary>
        /// Creates new customer in the system.
        /// </summary>
        /// <param name="data">Data for creation.</param>
        /// <returns>Id of newly created customer.</returns>
        public int CreateCustomer(CustomerData data)
        {
            using (var context = contextFactory.CreateContext())
            {
                var customer = Customer.Create(context.GetCustomers(), data.Name, data.Email);

                context.AddCustomer(customer);

                context.SaveChanges();

                return customer.Id;
            }
        }

        public async Task<int> CreateCustomerAsync(CustomerData data)
        {
            using (var context = contextFactory.CreateContext())
            {
                var customer = await Customer.CreateAsync(context.GetCustomers(), data.Name, data.Email);

                context.AddCustomer(customer);

                await context.SaveChangesAsync();

                return customer.Id;
            }
        }
    }
}