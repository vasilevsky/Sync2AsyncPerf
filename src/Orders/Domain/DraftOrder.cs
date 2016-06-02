using System;
using System.Collections.Generic;

namespace Domain
{
    public class DraftOrder
    {
        public DraftOrder(
            Product product,
            int number,
            decimal price,
            Units units,
            Currency currency,
            DateTime createdDate)
        {
            OrderLines = new List<OrderLine>();
            CreatedDate = createdDate;
            AddOrderLine(product, number, price, units, currency);
        }

        public decimal Price { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public ICollection<OrderLine> OrderLines { get; private set; }

        public void AddOrderLine(Product product, int number, decimal price, Units units, Currency currency)
        {
            OrderLines.Add(new OrderLine(product, number, units, price, currency));
        }
    }
}
