using CoffeeShop.Enums;
using CoffeeShop.Models;
using CoffeeShop.Repository;
using CoffeeShop.Service;
using CoffeeShop.View;
using System.Collections.Concurrent;


namespace CoffeeShop.Service
{
    internal class OrderService
    {
        private readonly OrdersRepo _ordersRepo;
        private readonly OrdersHistoryRepo _ordersHistoryRepo;
        public readonly List<Order> ActiveOrders;
        public readonly ConcurrentQueue<Order> ordersQueue;
        public readonly ConcurrentDictionary<Guid, CancellationTokenSource> orderCancellationTokenMapping;


        public OrderService(OrdersRepo ordersRepo, OrdersHistoryRepo ordersHistoryRepo)
        {
            this._ordersRepo = ordersRepo;
            this._ordersHistoryRepo = ordersHistoryRepo;
            this.ordersQueue = new ConcurrentQueue<Order>();
            this.orderCancellationTokenMapping = new();
            this.ActiveOrders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            ordersQueue.Enqueue(order);
        }

        public void PlaceOrder(MenuItem menuItem)
        {
            Guid orderId = Guid.NewGuid();
            Order order = new Order(orderId, menuItem, OrderStatus.Pending);
            orderCancellationTokenMapping[orderId] = new CancellationTokenSource();
            ActiveOrders.Add(order);
            this.AddOrder(order);
        }

        public bool TryGetOrder(out Order? order)
        {
            return this.ordersQueue.TryDequeue(out order);
        }

        public void AddHistory(Order order)
        {
            _ordersHistoryRepo.AddHistory(order);
        }

        public bool CancelOrder(Order order)
        {
            // cancellation mapping dictionary element should be removed 
            // after completion of the order
            // or cancellation of the order.
            if(order.Status != OrderStatus.Completed)
            {
                return false;
            }

            CancellationTokenSource cancelToken = orderCancellationTokenMapping[order.Id];
            cancelToken.Cancel();
            orderCancellationTokenMapping.TryRemove(order.Id, out _);
            ActiveOrders.Remove(order);
            return true;
        }
    }
}

