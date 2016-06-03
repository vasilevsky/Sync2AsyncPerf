using System;
using System.Data.Entity;
using System.Linq;
using Domain.Data;

namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Domain.Data.CustomerOrdersContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Domain.Data.CustomerOrdersContext context)
        {
            var chuck = Customer.Create("Chuck Noris", "chuck@example.com");
            chuck.AddOrder(78.35m);
            chuck.AddOrder(56.78m);
            context.Customers.Add(chuck);


            var jackie = Customer.Create("Jackie Chan", "jackie@example.com");
            jackie.AddOrder(78.35m);
            jackie.AddOrder(56.78m);
            context.Customers.Add(jackie);
        }
    }


    public class DataSeeding
    {
        private Random random = new Random();

        private readonly CustomerOrdersContext context;
        public static readonly DateTime StartDate = new DateTime(2006, 1, 1);
        public static readonly DateTime EndDate = new DateTime(2016, 1, 1);

        public DataSeeding(Data.CustomerOrdersContext context)
        {
            this.context = context;
        }

        public void Seed(int numberOfCustomers = 100, int numberOfProducts = 100)
        {
            SeedCustomers(numberOfCustomers);
            SeedProducts(numberOfProducts);
            var customers = context.Customers.ToArray();
            var products = context.Products.ToArray();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < numberOfCustomers; j++)
                {
                    SeedCustomerOrders(customers[j], products, numberOfProducts);
                }
            }
        }

        private void SeedCustomerOrders(Customer customer, Product[] products, int numberOfProducts)
        {
            var diff = EndDate - StartDate;
            var hours = random.Next(0, (int)diff.TotalHours);
            var orderDate = StartDate.AddHours(hours);

            var product1 = products[random.Next(numberOfProducts - 1)];
            var product2 = products[random.Next(numberOfProducts - 1)];
            var product3 = products[random.Next(numberOfProducts - 1)];
            var product4 = products[random.Next(numberOfProducts - 1)];

            var order1 = new DraftOrder(product1, 7, 17m, Units.liter, Currency.EUR, orderDate);
            order1.AddOrderLine(product2, 8, 2m, Units.kg, Currency.USD);
            order1.AddOrderLine(product3, 9, 4m, Units.oz, Currency.USD);
            order1.AddOrderLine(product4, 10, 6m, Units.pcs, Currency.USD);

            var order2 = new DraftOrder(product2, 7, 17m, Units.liter, Currency.EUR, orderDate);
            order2.AddOrderLine(product3, 8, 2m, Units.kg, Currency.USD);

            customer.AddOrder(order1);
            customer.AddOrder(order2);

            context.SaveChanges();
        }

        private void SeedCustomers(int numberOfCustomers)
        {
            for (int i = 0; i < numberOfCustomers; i++)
            {
                var customer = Customer.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                context.Customers.Add(customer);

                context.SaveChanges();
            }
        }

        private void SeedProducts(int numberOfProducts)
        {
            for (int i = 0; i < numberOfProducts; i++)
            {
                var product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                context.Products.Add(product);

                context.SaveChanges();
            }
        }
    }
}
