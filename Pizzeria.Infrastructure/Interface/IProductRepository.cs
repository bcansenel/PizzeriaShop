using Pizzeria.Infrastructure.Model;

namespace Pizzeria.Repository.Interface
{
    public interface IProductRepository
    {
        List<Product> GetProductPrices();

        Product? GetById(int productId);

        List<Ingredient> GetIngredients(int productId);
    }
}
