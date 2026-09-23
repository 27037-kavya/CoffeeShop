using CoffeeShop.Enums;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.View;

namespace CoffeeShop.Controller
{
    internal class CoffeeShopController
    {
        private readonly InventoryService _inventoryService;
        private readonly OrderService _orderService;
        private readonly PreparationService _preparationService;
        private readonly Dictionary<MainMenu, Action> _menuActions;

        public CoffeeShopController(InventoryService inventoryService, OrderService orderService, PreparationService preparationService)
        {
            this._inventoryService = inventoryService;
            this._orderService = orderService;
            this._preparationService = preparationService;
            this._menuActions = new Dictionary<MainMenu, Action>()
            {
                [MainMenu.MakeOrder] = this.MakeOrder,
                [MainMenu.CancelOrder] = this.CancelOrder,
                [MainMenu.Exit] = this.Exit,
            };

        }

        public void RunCoffeeChopApplication()
        {
            this._inventoryService.AddIngredients();
            _ = _inventoryService.RunRefillAsync();
            _ = _preparationService.RunScheduler();
            MainMenu option;
            do
            {
                option = CoffeeShopConsole.GetOption<MainMenu>();
                this._menuActions[option]();
            } while (option != MainMenu.Exit);


        }

        private void MakeOrder()
        {
            List<MenuItem> menu = this._inventoryService.GetMenuItems();
            CoffeeShopConsole.DisplayMenuItems(menu);
            int option = CoffeeShopConsole.GetMenuOption();
            MenuItem selectedItem = menu[option]; // need to be validated.
            _orderService.PlaceOrder(selectedItem);
        }

        private void CancelOrder()
        {
            List
            _orderService.CancelOrder();
        }

        private void Exit()
        {
            CoffeeShopConsole.DisplayMessage("Exiting...\nThank you:)");
        }
    }
}
