namespace API.Data
{
    public class ContextFactory : IReadOnlyContextFactory
    {
        public IReadOnlyContext CreateContext()
        {
            return new CustomerOrdersEntities();
        }
    }
}