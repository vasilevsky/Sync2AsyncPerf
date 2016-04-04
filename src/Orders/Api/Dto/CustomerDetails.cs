namespace API.Dto
{
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public OrderDetails[] Orders { get; set; }
    }
}