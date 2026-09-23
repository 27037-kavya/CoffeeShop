using CoffeeShop.Enums;
using CoffeeShop.Models;

namespace CoffeeShop.Repository
{
    internal class OrdersRepo
    {
        private readonly FileOperations<Order> _fileOperations;

        public OrdersRepo(FileOperations<Order> fileOperations)
        {
            _fileOperations = fileOperations;
        }

        public void AddOrders(Order order)
        {
            _fileOperations.AppendToFile(order);
        }

        public void UpdateStatus(Guid orderId, OrderStatus orderStatus)
        {
            List<Order> orders = this._fileOperations.ReadFromFile();
            Order? order = orders.FirstOrDefault(order => order.Id == orderId);
            if(order is not null)
            {
                order.status = orderStatus;
                _fileOperations.WriteToFile(orders);
            }
        }
    }
}
