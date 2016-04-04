using System.Data;

namespace Domain.Data
{
    public class CustomerOrderContextFactory : ICustomerOrdersContextFactory
    {
        public ICustomerOrdersContext CreateContext()
        {
            return new CustomerOrdersContext();
        }

        public ICustomerOrdersContext CreateContext(IsolationLevel isolationLevel)
        {
            return new CustomerOrdersContext(isolationLevel);
        }
    }
}
