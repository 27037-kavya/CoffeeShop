namespace CoffeeShop.Models
{
    internal class VendingMachine
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsBusy { get; set; }
    }
}
