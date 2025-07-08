namespace Pizzeria.Repository.Model
{
    internal class OrderEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DeliveryAddress { get; set; }
    }

    internal class OrderSummaryEntity
    {
        public int OrderId { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
