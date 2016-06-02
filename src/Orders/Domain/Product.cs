namespace Domain
{
    public class Product
    {
        private Product() { }

        public Product(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
