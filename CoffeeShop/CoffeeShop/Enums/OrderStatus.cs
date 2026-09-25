namespace CoffeeShop.Enums
{
    internal enum OrderStatus
    {
        Pending = 1,
        Completed,
        Processing,
        Cancelled,
        WaitingForMachine,
        WaitingForIngredients,
    }
}
