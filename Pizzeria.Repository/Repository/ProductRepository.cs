using CsvHelper;
using Pizzeria.Infrastructure.Model;
using Pizzeria.Repository.Interface;
using System.Globalization;
using System.Text.Json;

namespace Pizzeria.Repository.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> productPrices = new List<Product>();
        private readonly List<ProductIngredient> ingredients = new List<ProductIngredient>();
        public ProductRepository()
        {
            LoadProductPrices();
            LoadIngredients();
        }
        public List<Product> GetProductPrices()
        {
            return productPrices;
        }

        public Product? GetById(int productId)
        {
            return productPrices.Where(x => x.ProductId == productId)
                  .Select(x => x)
                  .FirstOrDefault();
        }
        public List<Ingredient> GetIngredients(int productId)
        {
            return ingredients.Where(x => x.ProductId == productId).SelectMany(x => x.Ingredients).ToList();
        }

        private void LoadProductPrices()
        {
            string[] productPriceFileNames = {
                "prices.json",
                "prices.csv"
            };
            foreach (var fileName in productPriceFileNames)
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    if (fileName.EndsWith(".json"))
                    {
                        string json = File.ReadAllText(filePath);
                        var prices = JsonSerializer.Deserialize<List<Product>>(json);
                        if (prices != null)
                        {
                            productPrices.AddRange(prices);
                        }
                    }
                    else if (fileName.EndsWith(".csv"))
                    {
                        using (var reader = new StreamReader(filePath))
                        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var prices = csv.GetRecords<Product>().ToList();
                            productPrices.AddRange(prices);
                        }
                    }
                    return;
                }
            }
        }

        private void LoadIngredients()
        {
            string[] ingredientsFileName = {
                 "ingredients.json",
                 "ingredients.csv"
             };
            foreach (var fileName in ingredientsFileName)
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    if (fileName.EndsWith(".json"))
                    {
                        var ingredientsJson = File.ReadAllText(filePath);
                        var productIngredients = JsonSerializer.Deserialize<List<ProductIngredient>>(ingredientsJson);

                        ingredients.AddRange(productIngredients ?? new List<ProductIngredient>());

                    }
                    else if (fileName.EndsWith(".csv"))
                    {
                        using var reader = new StreamReader(filePath);
                        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                        var ingredientsList = csv.GetRecords<ProductIngredient>().ToList();
                    }
                }
            }
        }

    }
}
