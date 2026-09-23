namespace CoffeeShop.Models
{
    internal class MenuItem // tea
    {
        public MenuItem(Guid id, string name, Dictionary<Guid, int> ingredients, int duration)
        {
            this.Id = id;
            this.Name = name;
            this.Ingredients = ingredients;
            this.Duration = duration;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        /// Ingredient id and its quantity.
        public Dictionary<Guid, int> Ingredients { get; set; } // milk, sugar

         // Time required to complete the item in seconds.
        public int Duration { get; set; }

    }
}
