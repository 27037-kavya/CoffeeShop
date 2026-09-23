using CoffeeShop.Enums;
using CoffeeShop.Models;

namespace CoffeeShop.Repository
{
    internal class OrdersHistoryRepo
    {
        private readonly FileOperations<Order> _fileOperations;

        public OrdersHistoryRepo(FileOperations<Order> fileOperations)
        {
            _fileOperations = fileOperations;
        }

        public void AddHistory(Order order)
        {
            _fileOperations.AppendToFile(order);
        }
    }
}
