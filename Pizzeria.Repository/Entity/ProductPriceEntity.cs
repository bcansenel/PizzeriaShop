﻿namespace Pizzeria.Repository.Entity
{
    internal class ProductPriceEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
