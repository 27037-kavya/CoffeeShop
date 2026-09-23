namespace CoffeeShop.Models
{
    internal class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public int MaxQuantity { get; init; }
    }
}
