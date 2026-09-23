using CoffeeShop.Models;
using CoffeeShop.Enums;
using System.Collections.Concurrent;

namespace CoffeeShop.Service
{
    internal class PreparationService
    {
        private readonly InventoryService _inventoryService;
        private readonly OrderService _orderService;
        private readonly ConcurrentDictionary<int, VendingMachine> _vendingMachines;

        public PreparationService(InventoryService inventoryService, OrderService orderService, ConcurrentDictionary<int, VendingMachine> vendingMachines)
        {
            this._inventoryService = inventoryService;
            this._orderService = orderService;
            this._vendingMachines = vendingMachines;
        }


        public async Task RunScheduler()
        {
            while (true)
            {
                foreach (var vendingMachines in this._vendingMachines)
                {
                    if (!vendingMachines.Value.IsBusy)
                    {
                        vendingMachines.Value.IsBusy = true;
                        _ = AllocateMachineAsync(vendingMachines.Value);
                    }
                }

                await Task.Delay(1000);
            }
        }

        public async Task AllocateMachineAsync(VendingMachine vendingMachine)
        {
            Order? orderToBeProcessed;
            try
            {
                if (_orderService.TryGetOrder(out orderToBeProcessed))
                {

                    if (orderToBeProcessed is null)
                    {
                        return;
                    }
                    CancellationTokenSource cts = new CancellationTokenSource();
                    vendingMachine.OrderId = orderToBeProcessed.Id;
                    await PrepareDishAsync(orderToBeProcessed, cts.Token);

                }
            }
            finally
            {
                vendingMachine.IsBusy = false;
                vendingMachine.OrderId = null;
            }
        }

        public async Task PrepareDishAsync(Order order, CancellationToken cancellationToken)
        {
            bool isIngredientsAllocated = false;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!this._inventoryService.ReduceIngredients(order.MenuItem.Ingredients))
                {
                    _orderService.AddOrder(order);
                    order.Status = OrderStatus.WaitingForIngredients;
                    return;
                }
                isIngredientsAllocated = true;
                cancellationToken.ThrowIfCancellationRequested();
                order.Status = OrderStatus.Processing;
                await Task.Delay(TimeSpan.FromSeconds(order.MenuItem.Duration), cancellationToken);
                order.Status = OrderStatus.Completed;
                this._orderService.AddHistory(order);
            }
            catch (OperationCanceledException)
            {
                CancelProcessing(order, isIngredientsAllocated);
                throw;
            }
        }

        public void CancelProcessing(Order order, bool isIngredientsAllocated)
        {
            if (isIngredientsAllocated)
            {
                this._inventoryService.IncreaseIngredients(order.MenuItem.Ingredients);
            }
            order.Status = OrderStatus.Cancelled;
        }
    }
}
