using CoffeeShop.Controller;
using CoffeeShop.Models;
using CoffeeShop.Repository;
using CoffeeShop.Service;
using CoffeeShop.View;



namespace CoffeeShop
{
    internal class Program
    {
        public static void Main()
        {
            FileOperations<Order> orderFileOperation = new FileOperations<Order>("orders.json");
            FileOperations<Order> orderHistoryFileOperation = new FileOperations<Order>("ordersHistory.json");
            FileOperations<Ingredient> ingredientsFileOperation = new("InventoryRepo.json");
            OrdersRepo orderRepo = new OrdersRepo(orderFileOperation);
            OrdersHistoryRepo ordersHistoryRepo = new OrdersHistoryRepo(orderHistoryFileOperation);
            IngredientsRepo ingredientsRepo = new IngredientsRepo(ingredientsFileOperation); 
            OrderService orderService = new OrderService(orderRepo, ordersHistoryRepo);
            List<VendingMachine> vendingMachines = new()
            {
                new VendingMachine(Guid.NewGuid(), "vm1", false, null),
                new VendingMachine(Guid.NewGuid(), "vm2", false, null),
                new VendingMachine(Guid.NewGuid(), "vm3", false, null),
            };
            NotificationService notificationService = new NotificationService();
            InventoryService inventoryService = new InventoryService(ingredientsRepo, notificationService);
            PreparationService preparationService = new PreparationService(inventoryService, orderService, vendingMachines, notificationService);
            CoffeeShopConsole console = new CoffeeShopConsole();
            CoffeeShopController controller = new CoffeeShopController(inventoryService, orderService, preparationService, notificationService, console);
            controller.RunCoffeeShopApplication();
        }
    }
}
