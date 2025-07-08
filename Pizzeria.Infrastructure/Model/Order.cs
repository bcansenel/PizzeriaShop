using Pizzeria.Infrastructure.Model;

namespace Pizzeria.Infrastructure.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
