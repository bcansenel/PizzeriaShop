using Pizzeria.Infrastructure.Interface;
using Pizzeria.Infrastructure.Model;
using Pizzeria.Repository.Interface;
using Pizzeria.Services.Interface;

namespace Pizzeria.Services.Service
{
    public class ProductService(IProductRepository productRepository, IOrderRepository orderRepository) : IProductService
    {
        public List<Ingredient> GetIngredients(int productId)
        {
            return productRepository.GetIngredients(productId) ??
                   throw new ArgumentNullException(nameof(productId), "No ingredients found for the specified product ID.");
        }

        public Product GetProduct(int productId)
        {
            return productRepository.GetById(productId) ??
                   throw new ArgumentNullException(nameof(productId), "No product price found for the specified product ID.");
        }

        public List<Product> GetProducts()
        {
            return productRepository.GetProductPrices();
        }
    }
}
