using CoffeeShop.Repository;

namespace CoffeeShop.Service
{
    internal class OrderService
    {
        private readonly OrdersRepo _ordersRepo;
        private readonly Queue _ordersQueue;


        public OrderService(OrdersRepo ordersRepo)
        {
            this._ordersRepo = ordersRepo;
        }

        public void PlaceOrder(OrdersRepo order)
        {
            _ordersQueue.Enqueue(order);
        }


    }
}
