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
        private readonly NotificationService _notifier;
        private readonly CoffeeShopConsole _console;

        public CoffeeShopController(InventoryService inventoryService, OrderService orderService, PreparationService preparationService, NotificationService notificationService, CoffeeShopConsole console)
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
            _notifier = notificationService;
            _console = console;
            _notifier.ShowNotification += _console.ShowNotification;

        }

        public void RunCoffeeShopApplication()
        {
            _console.Initialize();
            InitializeData();
            _inventoryService.RunRefill();
            _ = _preparationService.RunScheduler();
            MainMenu option;
            do
            {
                option = _console.GetOption<MainMenu>();
                this._menuActions[option]();

            } while (option != MainMenu.Exit);

        }

        private void MakeOrder()
        {
            List<MenuItem> menu = this._inventoryService.GetMenuItems();
            _console.DisplayMenuItems(menu);
            int option = _console.GetMenuOption();
            if (option <= 0 || option > menu.Count) 
            {
                _console.DisplayMessage("Invalid index");
                return;
            }
            MenuItem selectedItem = menu[option-1]; 
            this._orderService.PlaceOrder(selectedItem);
            _console.Refresh();

        }

        private void CancelOrder()
        {
            _console.DisplayOrders(this._orderService.ActiveOrders);
            if(this._orderService.ActiveOrders.Count == 0)
            {
                _console.DisplayMessage("No orders can be cancelled");
                return;
            }
            int sno = _console.GetMenuOption();
            if(sno <= 0 || sno > this._orderService.ActiveOrders.Count)
            {
                _console.DisplayMessage("Invalid index or the order has been completed.");
                return;
            }
            Guid id = MapSnoWithGuid(sno, this._orderService.ActiveOrders);
            Order order = this._orderService.ActiveOrders.First(o => o.Id == id);
            _orderService.CancelOrder(order);
            _console.DisplayMessage("Order cancelled...  :(");
            _console.Refresh();

        }

        private void Exit()
        {
            _console.DisplayMessage("Exiting...\nThank you:)");
        }

        private Guid MapSnoWithGuid<T>(int sno, List<T> list)
            where T : IHasId
        {
            return list[sno - 1].Id;
        }

        private void InitializeData()
        {
            if(!File.Exists("Ingredients.json"))
            {
                this._inventoryService.AddIngredients();
            }
        }
    }
}
