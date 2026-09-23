namespace CoffeeShop.Models
{
    internal class VendingMachine
    {
        public VendingMachine(Guid id, string name, bool isBusy, Guid? orderId)
        {
            this.Id = id;
            this.Name = name;
            this.IsBusy = isBusy;
            this.OrderId = orderId;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsBusy { get; set; }

        public Guid? OrderId { get; set; }
    }
}
