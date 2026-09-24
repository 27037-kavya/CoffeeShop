using CoffeeShop.Models;
using CoffeeShop.Enums;
using System.Collections.Concurrent;

namespace CoffeeShop.Service
{
    internal class PreparationService
    {
        private readonly InventoryService _inventoryService;
        private readonly OrderService _orderService;
        private readonly List<VendingMachine> _vendingMachines;

        public PreparationService(InventoryService inventoryService, OrderService orderService, List<VendingMachine> vendingMachines)
        {
            this._inventoryService = inventoryService;
            this._orderService = orderService;
            this._vendingMachines = vendingMachines;
        }

        public async Task RunScheduler()
        {
            while (true)
            {
                foreach (var vendingMachine in this._vendingMachines)
                {
                    if (!vendingMachine.IsBusy)
                    {
                        vendingMachine.IsBusy = true;
                        _ = AllocateMachineAsync(vendingMachine );
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
                    vendingMachine.OrderId = orderToBeProcessed.Id;
                    await PrepareDishAsync(orderToBeProcessed, _orderService.orderCancellationTokenMapping[orderToBeProcessed.Id].Token);

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
                this._orderService.orderCancellationTokenMapping.TryRemove(order.Id, out _);
                this._orderService.AddHistory(order);
                this._orderService.ActiveOrders.Remove(order);
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
