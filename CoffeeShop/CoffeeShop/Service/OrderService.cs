using CoffeeShop.Repository;
using System.Collections.Concurrent;
using CoffeeShop.Models;
using CoffeeShop.Enums;


namespace CoffeeShop.Service
{
    internal class OrderService
    {
        private readonly OrdersRepo _ordersRepo;
        private readonly OrdersHistoryRepo _ordersHistoryRepo;
        private readonly ConcurrentQueue<Order> _ordersQueue;


        public OrderService(OrdersRepo ordersRepo, OrdersHistoryRepo ordersHistoryRepo)
        {
            this._ordersRepo = ordersRepo;
            this._ordersHistoryRepo = ordersHistoryRepo;
            _ordersQueue = new ConcurrentQueue<Order>();
        }

        public void AddOrder(Order order)
        {
            _ordersQueue.Enqueue(order);
        }

        public void PlaceOrder(MenuItem menuItem)
        {
            Order order = new Order(Guid.NewGuid(), menuItem, OrderStatus.Pending);
            this.AddOrder(order);
        }

        public bool TryGetOrder(out Order? order)
        {
            return this._ordersQueue.TryDequeue(out order);
        }

        public void AddHistory(Order order)
        {
            _ordersHistoryRepo.AddHistory(order);
        }
    }
}
