using Pizzeria.Infrastructure.Model;

namespace Pizzeria.Services.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        List<Order> GetOrders();
        /// <summary>
        /// Gets an order by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the order.</param>
        /// <returns>The order with the specified identifier.</returns>
        Order GetSummaryById(int id);

        /// <summary>
        /// Gets a list of product IDs associated with a specific order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<int> GetOrderProductList(int orderId);

        List<OrderDetail> GetOrderDetails(int orderId);
    }
}
