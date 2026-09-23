using CoffeeShop.Enums;
using CoffeeShop.Models;
using CoffeeShop.View;
using System.Linq.Expressions;

namespace CoffeeShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CoffeeShopConsole view = new CoffeeShopConsole();
            view.DisplayMenu<MainMenu>();
        }
    }
}
