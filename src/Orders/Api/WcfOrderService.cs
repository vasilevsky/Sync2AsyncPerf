using System;
using System.Threading.Tasks;
using API.Services;

namespace API
{
    public class WcfOrderService
    {
        private static Lazy<OrderServiceClient> instance = new Lazy<OrderServiceClient>(() => new OrderServiceClient("BasicHttpBinding_IOrderService"));

        public static async Task AddOrderAsync(int customerId, decimal price)
        {
            await instance.Value.AsyncAddOrderAsync(customerId, price);
        }

        public static void AddOrder(int customerId, decimal price)
        {
            instance.Value.AddOrder(customerId, price);
        }
    }
}