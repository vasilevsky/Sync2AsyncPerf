﻿namespace Domain
{
    public class OrderLine
    {
        private OrderLine() { }

        public OrderLine(
            Product product,
            int number,
            Units units,
            decimal price,
            Currency currency)
        {
            Product = product;
            Number = number;
            Units = units;
            Price = price;
            Currency = currency;
        }

        public Product Product { get; private set; }

        public Units Units { get; private set; }

        public int Number { get; private set; }

        public decimal Price { get; private set; }

        public Currency Currency { get; private set; }
        public int Id { get; private set; }
    }

    public enum Currency
    {
        EUR,
        USD,
        UAH
    }

    public enum Units
    {
        kg,
        pcs,
        liter,
        oz
    }
}
