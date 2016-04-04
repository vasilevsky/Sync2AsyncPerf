using System.Threading.Tasks;

namespace API
{
    public interface ICustomerOrdersService
    {
        /// <summary>
        /// Adds order to customer.
        /// </summary>
        /// <param name="customerId">Id of customer.</param>
        /// <param name="price">Order price.</param>
        int AddOrder(int customerId, decimal price);
        Task<int> AddOrderAsync(int customerId, decimal price);

        /// <summary>
        /// Creates new customer to the system.
        /// </summary>
        /// <param name="data">Customer specific data/</param>
        /// <returns>Id if newly created customer.</returns>
        int CreateCustomer(CustomerData data);
        Task<int> CreateCustomerAsync(CustomerData data);
    }
}