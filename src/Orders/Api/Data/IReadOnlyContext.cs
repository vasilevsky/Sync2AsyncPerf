using System;
using System.Data.Entity;

namespace API.Data
{
    /// <summary>
    /// Describes readonly data context.
    /// </summary>
    public interface IReadOnlyContext : IDisposable
    {
        /// <summary>
        /// A set of Customers entities.
        /// </summary>
        DbSet<Customer> Customers { get; }
    }
}