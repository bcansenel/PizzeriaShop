using CsvHelper;
using Pizzeria.Infrastructure.Interface;
using Pizzeria.Infrastructure.Model;
using Pizzeria.Repository.Entity;
using Pizzeria.Repository.Model;
using System.Globalization;
using System.Text.Json;

namespace Pizzeria.Repository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<OrderSummaryEntity> _orders = new List<OrderSummaryEntity>();
        private readonly List<OrderDetailEntity> _orderDetails = new List<OrderDetailEntity>();

        public OrderRepository()
        {
            LoadOrders();
        }

        public Order GetById(int orderId)
        {
            var entity = _orders.FirstOrDefault(x => x.OrderId == orderId);
            if (entity == null)
                return null;

            // Map OrderSummaryEntity to OrderSummary
            return new Order
            {
                OrderId = entity.OrderId,
                DeliveryAt = entity.DeliveryAt,
                CreatedAt = entity.CreatedAt,
                DeliveryAddress = entity.DeliveryAddress,
                OrderDetails = _orderDetails
                    .Where(x => x.OrderId == entity.OrderId)
                    .Select(x => new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        OrderId = x.OrderId
                    })
                    .ToList()
            };
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetails
                .Where(x => x.OrderId == orderId)
                .Select(x => new OrderDetail
                {
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                })
                .ToList();
        }

        public List<int> GetOrderProductIds(int orderId)
        {
            return _orderDetails.Where(x => x.OrderId == orderId)
                .Select(x => x.ProductId)
                .ToList();
        }

        public List<Order> GetAll()
        {
            var orders = new List<Order>();

            foreach (var order in _orders)
            {
                var details = _orderDetails
                                        .Where(x => x.OrderId == order.OrderId)
                                        .ToList()
                                        .Select(x => new OrderDetail
                                        {
                                            ProductId = x.ProductId,
                                            Quantity = x.Quantity,
                                            OrderId = x.OrderId,
                                        }).ToList();

                orders.Add(new Order
                {
                    OrderId = order.OrderId,
                    DeliveryAt = order.DeliveryAt,
                    CreatedAt = order.CreatedAt,
                    DeliveryAddress = order.DeliveryAddress,
                    OrderDetails = details
                });
            }


            return orders;
        }

        private void LoadOrders()
        {
            string[] ordersFileName = {
            "orders.csv",
            "orders.json"
        };

            foreach (var fileName in ordersFileName)
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                if (File.Exists(filePath))
                {
                    if (fileName.EndsWith(".json"))
                    {
                        string json = File.ReadAllText(filePath);

                        var orders = JsonSerializer.Deserialize<List<OrderEntity>>(json);

                        var orderDetails = new List<OrderDetailEntity>();
                        foreach (var order in orders)
                        {
                            if (!_orders.Any(o => o.OrderId == order.OrderId))
                            {
                                _orders.Add(new OrderSummaryEntity
                                {
                                    OrderId = order.OrderId,
                                    DeliveryAt = order.DeliveryAt,
                                    CreatedAt = order.CreatedAt,
                                    DeliveryAddress = order.DeliveryAddress
                                });
                            }

                            _orderDetails.Add(new OrderDetailEntity
                            {
                                OrderId = order.OrderId,
                                ProductId = order.ProductId,
                                Quantity = order.Quantity
                            });
                        }

                    }
                    else if (fileName.EndsWith(".csv"))
                    {
                        using var reader = new StreamReader(filePath);
                        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                        var orderRecords = csv.GetRecords<OrderEntity>().ToList();

                        foreach (var order in orderRecords)
                        {
                            if (!_orders.Any(o => o.OrderId == order.OrderId))
                            {
                                _orders.Add(new OrderSummaryEntity
                                {
                                    OrderId = order.OrderId,
                                    DeliveryAt = order.DeliveryAt,
                                    CreatedAt = order.CreatedAt,
                                    DeliveryAddress = order.DeliveryAddress
                                });
                            }
                            _orderDetails.Add(new OrderDetailEntity
                            {
                                OrderId = order.OrderId,
                                ProductId = order.ProductId,
                                Quantity = order.Quantity
                            });
                        }

                    }

                    return;
                }
            }

        }

    }
}
