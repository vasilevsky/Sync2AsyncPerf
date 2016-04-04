namespace API.Data
{
    /// <summary>
    /// Describes factory for readonly data context.
    /// </summary>
    public interface IReadOnlyContextFactory
    {
        /// <summary>
        /// Creates the readonly context.
        /// </summary>
        /// <returns>Readonly data context.</returns>
        IReadOnlyContext CreateContext();
    }
}