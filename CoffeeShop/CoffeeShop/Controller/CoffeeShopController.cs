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

        public void RunCoffeeShopApplication()
        {
            this._inventoryService.AddIngredients();
            _ = _inventoryService.RunRefillAsync();
            _ = _preparationService.RunScheduler();
            MainMenu option;
            do
            {
                option = CoffeeShopConsole.GetOption<MainMenu>();
                this._menuActions[option]();
                // CoffeeShopConsole.Refresh();

            } while (option != MainMenu.Exit);

        }

        private void MakeOrder()
        {
            List<MenuItem> menu = this._inventoryService.GetMenuItems();
            CoffeeShopConsole.DisplayMenuItems(menu);
            int option = CoffeeShopConsole.GetMenuOption();
            if (option < 0 || option >= menu.Count)
            {
                CoffeeShopConsole.DisplayMessage("Invalid index");
                return;
            }
            MenuItem selectedItem = menu[option-1]; // need to be validated.
            this._orderService.PlaceOrder(selectedItem);
        }

        private void CancelOrder()
        {
            CoffeeShopConsole.DisplayOrders(this._orderService.ActiveOrders);
            if(this._orderService.ActiveOrders.Count == 0)
            {
                CoffeeShopConsole.DisplayMessage("No orders can be cancelled");
                return;
            }
            int sno = CoffeeShopConsole.GetMenuOption();
            if(sno < 0 || sno >= this._orderService.ActiveOrders.Count)
            {
                CoffeeShopConsole.DisplayMessage("Invalid index or the order has been completed.");
                return;
            }
            Guid id = MapSnoWithGuid(sno, this._orderService.ActiveOrders);
            Order order = this._orderService.ActiveOrders.First(o => o.Id == id);
            _orderService.CancelOrder(order);
        }

        private void Exit()
        {
            CoffeeShopConsole.DisplayMessage("Exiting...\nThank you:)");
        }

        private Guid MapSnoWithGuid<T>(int sno, List<T> list)
            where T : IHasId
        {
            return list[sno - 1].Id;
        }
    }
}
