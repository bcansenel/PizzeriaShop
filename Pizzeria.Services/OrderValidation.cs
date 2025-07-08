using Pizzeria.Infrastructure.Model;
using Pizzeria.Services.Service;

namespace Pizzeria.Services
{
    public static class OrderValidation
    {

        public static List<string> Validate(this Order order)
        {
            List<string> errors = new List<string>();

            ValidateAddress(order.DeliveryAddress, errors);
            ValidateDeliveryDate(order.DeliveryAt, errors);
            ValidateOrderDetails(order.OrderDetails, errors);
            return errors;
        }

        private static void ValidateAddress(string address, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                errors.Add("Delivery address cannot be empty.");
            }
            else if (address.Length < 5)
            {
                errors.Add("Delivery address must be at least 5 characters long.");
            }

        }

        private static void ValidateProduct(int productId, List<string> errors)
        {
            if (productId <= 0)
            {
                errors.Add($"Product ID must be greater than zero.");
            }

        }

        private static void ValidateQuantity(int quantity, List<string> errors)
        {
            if (quantity <= 0)
            {
                errors.Add($"Quantity must be greater than zero.");
            }
        }

        private static void ValidateDeliveryDate(DateTime? deliveryAt, List<string> errors)
        {
            if (deliveryAt == null)
            {
                errors.Add("Delivery date cannot be null.");
            }
        }

        private static void ValidateOrderDetails(List<OrderDetail> details, List<string> errors)
        {
            if (details == null || details.Count == 0)
            {
                errors.Add("Order detail cannot be null.");
                return;
            }

            foreach (var detail in details)
            {
                ValidateProduct(detail.ProductId, errors);
                ValidateQuantity(detail.Quantity, errors);
            }
        }
    }
}
