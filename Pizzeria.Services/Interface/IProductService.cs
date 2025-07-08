using Pizzeria.Infrastructure.Model;

namespace Pizzeria.Services.Interface
{
    public interface IProductService
    {

        List<Product> GetProducts();

        Product GetProduct(int productId);

        List<Ingredient> GetIngredients(int productId);

    }
}