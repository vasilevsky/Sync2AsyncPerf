using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Data
{
    /// <summary>
    /// Represents DbContext for domain entities.
    /// </summary>
    public class CustomerOrdersContext : DbContext, ICustomerOrdersContext
    {
        private readonly DbContextTransaction transaction;

        public const string Name = "CustomerOrders";

        /// <summary>
        /// Initializes context with default Isolation level.
        /// </summary>
        public CustomerOrdersContext()
            : base(Name)
        {
            transaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Initializes context with specified Isolation level.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level.</param>
        public CustomerOrdersContext(IsolationLevel isolationLevel)
            : base(Name)
        {
            transaction = Database.BeginTransaction(isolationLevel);
        }

        public DbSet<Customer> Customers { get; set; }

        public Customer GetCustomer(int id)
        {
            return Customers
                .Include(m => m.Orders)
                .SingleOrDefault(m => m.Id == id);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await Customers
                .Include(m => m.Orders)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        public IQueryable<Customer> GetCustomers()
        {
            return Customers;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfig());
            modelBuilder.Configurations.Add(new OrderConfig());

            base.OnModelCreating(modelBuilder);
        }

        void IDisposable.Dispose()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }

            base.Dispose();
        }

        private class CustomerConfig : EntityTypeConfiguration<Customer>
        {
            public CustomerConfig()
            {
                ToTable("Customers");
                HasKey(m => m.Id).Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(m => m.Name).IsRequired().HasMaxLength(200);
                Property(m => m.Email).IsRequired().HasMaxLength(200);
                HasMany(m => m.Orders);
            }
        }

        private class OrderConfig : EntityTypeConfiguration<Order>
        {
            public OrderConfig()
            {
                ToTable("Orders");
                HasKey(m => m.Id).Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(m => m.Price).IsRequired();
                Property(m => m.CreatedDate).IsRequired();
                HasRequired(m => m.Customer);
            }
        }
    }
}
