using System.Threading.Tasks;
using API;
using Domain.Data;

namespace Wcf
{
    public class OrderService : IOrderService
    {
        CustomerOrdersService service = new CustomerOrdersService(new CustomerOrderContextFactory());

        public void AddOrder(int customerId, decimal price)
        {
            service.AddOrder(customerId, price);
        }

        public async Task AsyncAddOrder(int customerId, decimal price)
        {
            await service.AddOrderAsync(customerId, price);
        }
    }
}
