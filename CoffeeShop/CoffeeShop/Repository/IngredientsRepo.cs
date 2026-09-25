using CoffeeShop.Models;

namespace CoffeeShop.Repository
{
    internal class IngredientsRepo
    {
        private readonly FileOperations<Ingredient> _fileOperations;

        public IngredientsRepo(FileOperations<Ingredient> fileOperations)
        {
            this._fileOperations = fileOperations;
        }

        public void UpdateIngredientQuantity(Guid id, int quantity)
        {
            List<Ingredient> ingredients = this._fileOperations.ReadFromFile();
            Ingredient? ingredient = ingredients.FirstOrDefault(item => item.Id == id);
            if(ingredient is not null)
            {
                ingredient.Quantity = quantity;
                this._fileOperations.WriteToFile(ingredients);
            }
        }

        public List<Ingredient> GetAllIngredients()
        {
            return this._fileOperations.ReadFromFile();
        }

        public void UpdateAllIngredients(List<Ingredient> ingredients)
        {
            this._fileOperations.WriteToFile(ingredients);
        }

        public void AddIngredient(Ingredient ingredient)
        {
            this._fileOperations.AppendToFile(ingredient);
        }
    }
}
 