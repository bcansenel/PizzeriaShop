namespace Pizzeria.Infrastructure.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
