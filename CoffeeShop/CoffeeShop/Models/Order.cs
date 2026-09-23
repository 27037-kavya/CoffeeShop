using CoffeeShop.Enums;

namespace CoffeeShop.Models
{
    internal class Order
    {
        public Order(Guid id, MenuItem menuItem, OrderStatus status)
        {
            this.Id = id;
            this.MenuItem = menuItem;
            this.Status = status;
        }
        public Guid Id { get; set; }

        public MenuItem MenuItem { get; set; }

        public OrderStatus Status { get; set; }
    }
}
