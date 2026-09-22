using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Service
{
    internal class InventoryService
    {
        private readonly IngredientsRepo _ingredientsRepo;
        public InventoryService(IngredientsRepo ingredientsRepo)
        {
            _ingredientsRepo = ingredientsRepo;
        }


        public void Refill()
        {
            List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
            foreach(Ingredient ingredient in ingredients)
            {
                if(ingredient.Quantity < ingredient.MaxQuantity)
                {
                    ingredient.Quantity = ingredient.MaxQuantity;
                }
            }
            this._ingredientsRepo.UpdateAllIngredients(ingredients);
        }

        public void ReduceIngredients(Dictionary<Guid, int> ingredientsWithQuantity)
        {
            List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();

            foreach(var item in ingredientsWithQuantity)
            {
                Ingredient? ingredientToBeReduced = ingredients.FirstOrDefault(ingredient => ingredient.Id == item.Key);
                if(ingredientToBeReduced is null)
                {
                    throw new InvalidOperationException("Unknown ingredient required");
                }

                if (ingredientToBeReduced.Quantity >= item.Value)
                {
                    ingredientToBeReduced.Quantity -= item.Value;
                }
                else
                {

                }
            }
        }

        
    }
}
