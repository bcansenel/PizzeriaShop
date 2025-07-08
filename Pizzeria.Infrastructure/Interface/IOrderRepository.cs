using Pizzeria.Infrastructure.Model;

namespace Pizzeria.Infrastructure.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAll();

        Order GetById(int orderId);

        List<OrderDetail> GetOrderDetails(int orderId);
        List<int> GetOrderProductIds(int orderId);

    }
}
