namespace Pizzeria.Infrastructure.Model;

public class ProductIngredient
{
    public int ProductId { get; set; }

    public List<Ingredient> Ingredients { get; set; }

}
