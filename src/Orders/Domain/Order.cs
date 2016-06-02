using System;
using System.Collections.Generic;

namespace Domain
{
    public class Order
    {
        private Order()
        { }

        private Order(decimal price, DateTime createdDate)
        {
            Price = price;
            CreatedDate = createdDate;
        }

        public Order(Customer customer, DraftOrder draft)
        {
            Customer = customer;
            Price = draft.Price;
            OrderLines = draft.OrderLines;
            CreatedDate = draft.CreatedDate;
        }

        public int Id { get; private set; }

        public decimal Price { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public Customer Customer { get; private set; }

        public ICollection<OrderLine> OrderLines { get; private set; }

        /// <summary>
        /// Creates instance of Order.
        /// </summary>
        /// <param name="price">Order price.</param>
        /// <exception cref="ArgumentException">Thrown when price is non positive.</exception>
        /// <returns>Newly created order instance.</returns>
        public static Order Create(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price should be more than zero", "price");
            }

            return new Order(price, DateTime.UtcNow);
        }
    }
}
