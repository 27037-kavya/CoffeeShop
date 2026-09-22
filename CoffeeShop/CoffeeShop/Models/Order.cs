using CoffeeShop.Enums;

namespace CoffeeShop.Models
{
    internal class Order
    {
        public Guid Id { get; set; }

        public MenuItem menuItem { get; set; }

        public OrderStatus status { get; set; }
    }
}
