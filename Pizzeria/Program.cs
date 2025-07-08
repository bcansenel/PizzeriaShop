
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pizzeria.Repository.Extension;
using Pizzeria.Services;
using Pizzeria.Services.Service;
using Pizzeria.Services.Interface;
using Pizzeria.Infrastructure.Model;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
                            .ConfigureServices(services =>
                            {
                                // register services from class library
                                services.AddPizzeriaServices();
                                services.AddRepositories();
                            })
                            .Build();

        var orderService = host.Services.GetRequiredService<IOrderService>();

        var orders = orderService.GetOrders();

        if (orders == null || orders.Count == 0)
        {
            Console.WriteLine("No orders to process.");
            return;
        }

        var productService = host.Services.GetRequiredService<ProductService>();
        var productPrices = productService.GetProducts();
        if (productPrices == null || productPrices.Count == 0)
        {
            Console.WriteLine("No product prices available.");
            return;
        }

        var ingredientTotalQuantity = new List<IngredientQuantity>();

        foreach (var order in orders)
        {
            var errors = order.Validate();

            if (errors.Any())
            {
                PrintValidationErrors(errors);
                continue;
            }

            Console.WriteLine($"  ***** Order : {order.OrderId} *****");

            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
            {
                Console.WriteLine($"  No order details found for order {order.OrderId}.");
                continue;
            }

            decimal totalPrice = 0;
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = productService.GetProduct(orderDetail.ProductId);
                if (product != null)
                {
                    totalPrice += product.Price * orderDetail.Quantity;
                    PrintOrderDetail(orderDetail, product);
                    var ingredients = productService.GetIngredients(product.ProductId);
                    if (ingredients == null || ingredients.Count == 0)
                    {
                        Console.WriteLine($"  No ingredients found for product {product.ProductName} (ProductId: {product.ProductId}).");
                        continue;
                    }
                    CalculateTotalRequiredIngridients(ingredients, ingredientTotalQuantity, orderDetail.Quantity);
                }
            }
            Console.WriteLine(new string(' ', 55) + $"Total Price : {totalPrice:C}");
            PrintOrderSeperator();
        }

        PrintTotalRequiredIngredients(ingredientTotalQuantity);
    }

    private static void PrintTotalRequiredIngredients(List<IngredientQuantity> ingredientTotalQuantity)
    {
        Console.WriteLine("\nTotal Ingredients Used Across All Orders:\n");

        foreach (var ingredient in ingredientTotalQuantity)
        {
            Console.WriteLine($"    Ingredient: {ingredient.IngredientName} | Total Amount: {ingredient.Amount} gr");
        }
    }

    private static void CalculateTotalRequiredIngridients(List<Ingredient> ingredients,
                                                     List<IngredientQuantity> ingredientTotalQuantity,
                                                     int quantity)
    {
        if (ingredients != null)
        {
            foreach (var ingredient in ingredients)
            {
                if (ingredientTotalQuantity.Any(x => x.IngredientName == ingredient.Name))
                {
                    var existingIngredient = ingredientTotalQuantity.First(x => x.IngredientName == ingredient.Name);
                    existingIngredient.Amount += ingredient.Amount * quantity;
                }
                else
                {
                    ingredientTotalQuantity.Add(new IngredientQuantity
                    {
                        IngredientName = ingredient.Name,
                        Amount = ingredient.Amount * quantity
                    });
                }
            }
        }
    }

    private static void PrintOrderDetail(OrderDetail orderDetail, Product product)
    {
        Console.WriteLine($"     ==> | {product.ProductName} | Qty: {orderDetail.Quantity} | UnitPrice: {product.Price:C} | LineTotal: {product.Price * orderDetail.Quantity:C}");
    }

    private static void PrintOrderSeperator()
    {
        Console.WriteLine(new string('-', 100) + "\n");
    }
    private static void PrintValidationErrors(List<string> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($" - {error}");
        }
    }
}

//internal class Program
//{
//private static void Main(string[] args)
//{

//}

//private static readonly string baseDirectory = "..\\Pizzeria\\Pizzeria\\Files\\"; //Path.Combine(AppContext.BaseDirectory, "Pizzeria", "Files");
//private static void Main(string[] args)
//{
//    // Read Orders
//    var orders = LoadOrders();

//    // If no orders are found, exit the program
//    if (orders == null)
//    {
//        Console.WriteLine("No orders to process.");
//        return;
//    }
//    // Read Product Prices
//    var productPrices = LoadPriceList();

//    // If no product prices are found, exit the program
//    if (productPrices == null || productPrices.Count == 0)
//    {
//        Console.WriteLine("No product prices available.");
//        return;
//    }

//    var productIngredients = LoadIngredients();
//    // If no ingredients are found, exit the program
//    if (productIngredients == null || productIngredients.Count == 0)
//    {
//        Console.WriteLine("No ingredients available.");
//        return;
//    }

//    //Get Order Summary
//    List<OrderSummary> orderSummary = GetOrderSummary(orders, productPrices);

//    List<OrderBaseIngredientInformation>? listOfTotalIngredients = new List<OrderBaseIngredientInformation>();

//    foreach (var order in orderSummary)
//    {
//        Console.WriteLine($" OrderId: {order.OrderId}");
//        foreach (var detail in order.Details)
//        {
//            Console.WriteLine($" - ProductId: {detail.ProductId} | {detail.ProductName} | Qty: {detail.Quantity} | UnitPrice: {detail.Price:C} | LineTotal: {detail.Total:C}");

//            productIngredients.Where(x => x.ProductId == detail.ProductId)
//                .SelectMany(x => x.Ingredients)
//                .ToList()
//                .ForEach(ingredient =>
//                {
//                    listOfTotalIngredients.Add(new OrderBaseIngredientInformation
//                    {
//                        Ingredient = ingredient,
//                        TotalAmount = ingredient.Amount * detail.Quantity,
//                        ProductId = detail.ProductId,
//                        OrderId = order.OrderId,
//                        ProductOrderCount = detail.Quantity
//                    });
//                });
//        }

//        Console.WriteLine($"Order Total: {order.OrderTotal:C}");
//    }


//    //List<(Ingredient Ingredient, decimal totalAmount, int productId)>? listOfTotalIngredients = new List<(Ingredient Ingredient, decimal totalAmount, int productId)>();

//    //foreach (var order in orderSummary)
//    //{
//    //    Console.WriteLine($" OrderId: {order.OrderId}");
//    //    foreach (var detail in order.Details)
//    //    {
//    //        Console.WriteLine($" - ProductId: {detail.ProductId} | {detail.ProductName} | Qty: {detail.Quantity} | UnitPrice: {detail.Price:C} | LineTotal: {detail.Total:C}");

//    //        productIngredients.Where(x => x.ProductId == detail.ProductId)
//    //            .SelectMany(x => x.Ingredients)
//    //            .ToList()
//    //            .ForEach(ingredient =>
//    //            {
//    //                listOfTotalIngredients.Add((ingredient, (ingredient.Amount * detail.Quantity), detail.ProductId));
//    //            });
//    //    }

//    //    Console.WriteLine($"Order Total: {order.OrderTotal:C}");
//    //}


//    Console.WriteLine("\nTotal Ingredients Used Across All Orders:\n");

//    listOfTotalIngredients
//        .GroupBy(x => x.Ingredient.Id)
//        .Select(group => new
//        {
//            Ingredient = group.First().Ingredient,
//            TotalAmount = group.Sum(x => x.TotalAmount),
//            ProductId = group.First().ProductId,
//            OrderId = group.First().OrderId,
//        })
//        .ToList()
//        .ForEach(ingredient =>
//        {
//            Console.WriteLine($"    Ingredient: {ingredient.Ingredient.Name} | Total Amount: {ingredient.TotalAmount} gr");
//        });


//}

//private static List<OrderSummary> GetOrderSummary(List<Order> orders, List<ProductPrice> productPrices)
//{
//    return orders
//        .Join(productPrices,
//              o => o.ProductId,
//              p => p.ProductId,
//              (o, p) => new
//              {
//                  o.OrderId,
//                  o.ProductId,
//                  p.ProductName,
//                  o.Quantity,
//                  p.Price,
//                  Total = o.Quantity * p.Price
//              })
//        .GroupBy(x => x.OrderId)
//        .Select(g => new OrderSummary
//        {
//            OrderId = g.Key,
//            Details = g.Select(x => new OrderDetail
//            {
//                ProductId = x.ProductId,
//                ProductName = x.ProductName,
//                Quantity = x.Quantity,
//                Price = x.Price,
//                Total = x.Total
//            }).ToList(),
//            OrderTotal = g.Sum(x => x.Total)
//        })
//        .ToList();
//}

//private static List<Order>? LoadOrders()
//{
//    string[] ordersFileName = {
//        "orders.json",
//        "orders.csv"
//    };
//    foreach (var fileName in ordersFileName)
//    {
//        var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

//        if (File.Exists(filePath))
//        {
//            if (fileName.EndsWith(".json"))
//            {
//                string json = File.ReadAllText(filePath);

//                var orders = JsonSerializer.Deserialize<List<Order>>(json);

//                return orders;

//            }
//            else if (fileName.EndsWith(".csv"))
//            {
//                using var reader = new StreamReader(filePath);
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//                var orderRecords = csv.GetRecords<Order>().ToList();
//                return orderRecords;
//            }
//        }
//    }

//    return null;
//}

//private static List<ProductPrice>? LoadPriceList()
//{
//    string[] priceListFileName = {
//        "prices.json",
//        "prices.csv"
//    };

//    foreach (var fileName in priceListFileName)
//    {
//        var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

//        if (File.Exists(filePath))
//        {
//            if (fileName.EndsWith(".json"))
//            {
//                var priceListJson = File.ReadAllText(filePath);

//                var productPrices = JsonSerializer.Deserialize<List<ProductPrice>>(priceListJson);

//                return productPrices;
//            }
//            else if (fileName.EndsWith(".csv"))
//            {
//                using var reader = new StreamReader(filePath);
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//                var priceList = csv.GetRecords<ProductPrice>().ToList();
//                return priceList;
//            }
//        }
//    }
//    return null;
//}

//private static List<ProductIngredients> LoadIngredients()
//{
//    string[] ingredientsFileName = {
//        "ingredients.json",
//        "ingredients.csv"
//    };
//    foreach (var fileName in ingredientsFileName)
//    {
//        var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
//        if (File.Exists(filePath))
//        {
//            if (fileName.EndsWith(".json"))
//            {
//                var ingredientsJson = File.ReadAllText(filePath);
//                var productIngredients = JsonSerializer.Deserialize<List<ProductIngredients>>(ingredientsJson);
//                return productIngredients;
//            }
//            else if (fileName.EndsWith(".csv"))
//            {
//                using var reader = new StreamReader(filePath);
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//                var ingredientsList = csv.GetRecords<ProductIngredients>().ToList();
//                return ingredientsList;
//            }
//        }
//    }
//    return null;
//}
//}