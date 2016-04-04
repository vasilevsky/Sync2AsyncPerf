using System.Data.Entity;
using System.Data.Entity.Migrations;
using Domain.Data;

namespace Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CustomerOrdersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(CustomerOrdersContext context)
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
}
