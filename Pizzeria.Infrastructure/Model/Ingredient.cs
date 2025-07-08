namespace Pizzeria.Infrastructure.Model
{
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
