using Pizzeria.Infrastructure.Interface;
using Pizzeria.Infrastructure.Model;
using Pizzeria.Services.Interface;

namespace Pizzeria.Services.Service
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public List<Order> GetOrders()
        {
            return orderRepository.GetAll();
        }
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return orderRepository.GetOrderDetails(orderId);
        }

        public List<int> GetOrderProductList(int orderId)
        {
            return orderRepository.GetOrderProductIds(orderId);
        }

        public Order GetSummaryById(int id)
        {
            return orderRepository.GetById(id);
        }
    }
}
