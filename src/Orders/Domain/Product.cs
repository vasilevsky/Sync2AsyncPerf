namespace Domain
{
    public class Product
    {
        private Product() { }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
