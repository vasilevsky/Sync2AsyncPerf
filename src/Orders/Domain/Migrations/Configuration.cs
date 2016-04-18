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
}
