using System.Data;

namespace Domain.Data
{
    /// <summary>
    /// Represents factory of CustomerOrdersContext.
    /// </summary>
    public interface ICustomerOrdersContextFactory
    {
        /// <summary>
        /// Creates context.
        /// </summary>
        /// <returns>Instance of created context.</returns>
        ICustomerOrdersContext CreateContext();

        /// <summary>
        /// Creates context with specified isolation.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level.</param>
        /// <returns>Instance of created context.</returns>
        ICustomerOrdersContext CreateContext(IsolationLevel isolationLevel);
    }
}
