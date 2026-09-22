namespace CoffeeShop.Models
{
    internal class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// Ingredient id and its quantity.
        public Dictionary<Guid, int> Ingredients { get; set; }

         // Time required to complete the item in seconds.
        public int Duration { get; set; }

    }
}
