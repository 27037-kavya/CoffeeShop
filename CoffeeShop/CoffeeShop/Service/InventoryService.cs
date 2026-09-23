using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Service
{
    internal class InventoryService
    {
        private readonly IngredientsRepo _ingredientsRepo;
        private readonly object _ingredientsLock;
        private readonly List<MenuItem> _menuItems;

        public InventoryService(IngredientsRepo ingredientsRepo)
        {
            _ingredientsRepo = ingredientsRepo;
            _ingredientsLock = new object();

            Guid coffeePowderId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid milkId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            Guid sugarId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            Guid chocolateSyrupId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            _menuItems = new List<MenuItem>
            {
                new MenuItem(
            Guid.NewGuid(),
            "Espresso",
            new Dictionary<Guid, int>
            {
                { coffeePowderId, 3 }
            },
            3),

                new MenuItem(
            Guid.NewGuid(),
            "Black Coffee",
            new Dictionary<Guid, int>
            {
                { coffeePowderId, 2 },
                { sugarId, 1 }
            },
            4),

                new MenuItem(
            Guid.NewGuid(),
            "Coffee",
            new Dictionary<Guid, int>
            {
                { coffeePowderId, 2 },
                { milkId, 2 },
                { sugarId, 1 }
            },
            5),

                new MenuItem(
            Guid.NewGuid(),
            "Cappuccino",
            new Dictionary<Guid, int>
            {
                { coffeePowderId, 2 },
                { milkId, 3 },
                { sugarId, 1 }
            },
            8),

                new MenuItem(
            Guid.NewGuid(),
            "Mocha",
            new Dictionary<Guid, int>
            {
                { coffeePowderId, 2 },
                { milkId, 2 },
                { sugarId, 1 },
                { chocolateSyrupId, 1 }
            },
            10)
            };
        }


        // Refill inventory every 30 minutes.
        public async Task RunRefillAsync()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMinutes(30));
                Refill();
            }
        }

        public void Refill()
        {
            lock (_ingredientsLock)
            {
                List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
                // Repo logic
                foreach (Ingredient ingredient in ingredients)
                {
                    if (ingredient.Quantity < ingredient.MaxQuantity)
                    {
                        ingredient.Quantity = ingredient.MaxQuantity;
                    }
                }
                this._ingredientsRepo.UpdateAllIngredients(ingredients);
            }
        }

        public bool ReduceIngredients(Dictionary<Guid, int> ingredientsWithQuantity)
        {
            lock (_ingredientsLock)
            {
                List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
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


                // Repo logic
                foreach (var item in ingredientsWithQuantity)
                {
                    Ingredient? ingredientToBeReduced = ingredients.FirstOrDefault(ingredient => ingredient.Id == item.Key);
                    if (ingredientToBeReduced is null)
                    {
                        throw new InvalidOperationException("Unknown ingredient required");
                    }
                    ingredientToBeReduced.Quantity -= item.Value;
                }
                this._ingredientsRepo.UpdateAllIngredients(ingredients);

                return true;
            }
        }


        public void IncreaseIngredients(Dictionary<Guid, int> ingredientsWithQuantity)
        // will be used if any order is cancelled after the ingredients allocation.
        {
            lock (_ingredientsLock)
            {
                List<Ingredient> ingredients = _ingredientsRepo.GetAllIngredients();
                foreach (var item in ingredientsWithQuantity)
                {
                    Ingredient ingredientToBeUpdated = ingredients.First(ingredient => ingredient.Id == item.Key);
                    int updatedQuantity = ingredientToBeUpdated.Quantity + item.Value;

                    // Handles if the updatedQuantity greater than maxQuantity,
                    // may occur if the refill happened after the stock reduction.
                    ingredientToBeUpdated.Quantity = Math.Min(updatedQuantity, ingredientToBeUpdated.MaxQuantity);
                }
                this._ingredientsRepo.UpdateAllIngredients(ingredients);
            }
        }


        public void AddIngredients()
        {
            _ingredientsRepo.AddIngredient(
                new Ingredient(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Coffee Powder",
                    100,
                    100));

            _ingredientsRepo.AddIngredient(
                new Ingredient(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Milk",
                    100,
                    100));

            _ingredientsRepo.AddIngredient(
                new Ingredient(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Sugar",
                    100,
                    100));

            _ingredientsRepo.AddIngredient(
                new Ingredient(
                    Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    "Chocolate Syrup",
                    50,
                    50));
        }

        public List<MenuItem> GetMenuItems()
        {
            return _menuItems;
        }


    }
}
