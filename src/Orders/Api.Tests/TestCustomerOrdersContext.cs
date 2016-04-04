using System.Data;
using Domain.Data;

namespace API.Tests
{
    /// <summary>
    /// Implements test factory of ICustomerOrdersContext.
    /// </summary>
    public class TestCustomerOrdersContextFactory : ICustomerOrdersContextFactory
    {
        private readonly ICustomerOrdersContext context;

        public TestCustomerOrdersContextFactory(ICustomerOrdersContext context)
        {
            this.context = context;
        }

        public ICustomerOrdersContext CreateContext(IsolationLevel isolationLevel)
        {
            return context;
        }

        public ICustomerOrdersContext CreateContext()
        {
            return context;
        }
    }
}
