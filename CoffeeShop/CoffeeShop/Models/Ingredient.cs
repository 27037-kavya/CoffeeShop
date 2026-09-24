namespace CoffeeShop.Models
{
    internal class Ingredient : IHasId
    {
        public Ingredient(Guid id, string name, int quantity, int maxQuantity)
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
            this.MaxQuantity = maxQuantity;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;


        public int Quantity { get; set; }

        public int MaxQuantity { get; init; }
    }
}
