using System.ServiceModel;
using System.Threading.Tasks;

namespace Wcf
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        void AddOrder(int customerId, decimal price);

        [OperationContract]
        Task AsyncAddOrder(int customerId, decimal price);
    }
}
