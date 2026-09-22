using CoffeeShop.Models;

namespace CoffeeShop.Service
{
    internal class PreparationService
    {
        private readonly InventoryService _inventoryService;
        private readonly OrderService _orderService;

        public PreparationService(InventoryService inventoryService, OrderService orderService)
        {
            this._inventoryService = inventoryService;
            this._orderService = orderService;
        }

        public async Task PrepareDishAsync(MenuItem menuItem)
        {
            if(this._inventoryService.ReduceIngredients(menuItem.Ingredients))
            {
                await Task.Delay(menuItem.Duration);
            }
            else
            {
                _orderService.AddQueue();
            }
        }
    }
}
