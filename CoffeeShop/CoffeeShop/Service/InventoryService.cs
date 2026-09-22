using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Service
{
    internal class InventoryService
    {
        private readonly IngredientsRepo _ingredientsRepo;
        private readonly object _ingredientsLock;
        public InventoryService(IngredientsRepo ingredientsRepo)
        {
            _ingredientsRepo = ingredientsRepo;
            _ingredientsLock = new object();
        }


        public void Refill()
        {
            List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
            foreach (Ingredient ingredient in ingredients)
            {
                if (ingredient.Quantity < ingredient.MaxQuantity)
                {
                    ingredient.Quantity = ingredient.MaxQuantity;
                }
            }
            this._ingredientsRepo.UpdateAllIngredients(ingredients);
        }

        public bool ReduceIngredients(Dictionary<Guid, int> ingredientsWithQuantity)
        {
            List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();

            lock (_ingredientsLock)
            {
                foreach (var item in ingredientsWithQuantity)
                {
                    Ingredient? ingredientToBeReduced = ingredients.FirstOrDefault(ingredient => ingredient.Id == item.Key);
                    if (ingredientToBeReduced is null)
                    {
                        throw new InvalidOperationException("Unknown ingredient required");
                    }

                    if (ingredientToBeReduced.Quantity < item.Value)
                    {
                        return false;
                    }
                }

                foreach (var item in ingredientsWithQuantity)
                {
                    Ingredient? ingredientToBeReduced = ingredients.FirstOrDefault(ingredient => ingredient.Id == item.Key);
                    if (ingredientToBeReduced is null)
                    {
                        throw new InvalidOperationException("Unknown ingredient required");
                    }
                    ingredientToBeReduced.Quantity -= item.Value;
                }
                return true;
            }
        }


        public void IncreaseIngredients(Dictionary<Guid, int> ingredientsWithQuantity) 
            // will be used if any order is cancelled after the ingredients allocation.
        {
            List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
            lock(_ingredientsLock)
            {
                foreach(var item in ingredientsWithQuantity)
                {
                    Ingredient ingredientToBeUpdated = ingredients.First(ingredient => ingredient.Id == item.Key);
                    int updatedQuantity = ingredientToBeUpdated.Quantity + item.Value;

                    // Handles if the updatedQuantity greater than maxQuantity,
                    // may occur if the refill happened after the stock reduction.
                    ingredientToBeUpdated.Quantity = Math.Min(updatedQuantity, ingredientToBeUpdated.MaxQuantity);
                }
            }
        }


    }
}
