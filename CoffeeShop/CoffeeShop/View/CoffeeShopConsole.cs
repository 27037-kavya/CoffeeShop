using CoffeeShop.Models;
using CoffeeShop.Validator;
using System.Text;
using System.Text.RegularExpressions;

namespace CoffeeShop.View
{
    internal class CoffeeShopConsole
    {

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);

        }

        public static void DisplayMenu<T>()
            where T : Enum
        {
            StringBuilder menuBuilder = new StringBuilder();
            foreach (T menuItem in Enum.GetValues(typeof(T)))
            {
                string readableName = Regex.Replace(menuItem.ToString(), "([a-z])([A-Z])", "$1 $2");
                menuBuilder.AppendLine($"{Convert.ToInt32(menuItem)}. {readableName}");
            }

            DisplayMessage(menuBuilder.ToString().TrimEnd());

        }

        public static T GetOption<T>()
            where T : struct, Enum
        {
            DisplayMenu<T>();
            T? validatedOption;
            do
            {
                string userInput = Console.ReadLine()?.Trim() ?? string.Empty;

                if (!InputValidator.IsValiOption<T>(userInput, out validatedOption))
                {
                    Console.WriteLine("Invalid option.");
                    continue;
                }
                return validatedOption ?? default;
            } while (true);
        }

        public static void DisplayMenuItems(List<MenuItem> menu)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menu[i].Name}");
            }
        }

        public static int GetMenuOption()
        {
            while (true)
            {
                string userOption = Console.ReadLine()?.Trim() ?? string.Empty;
                if (userOption == string.Empty)
                {
                    Console.WriteLine("Cannot be empty");
                }
                else if (int.TryParse(userOption, out int option))
                {
                    return option;
                }
                Console.WriteLine("Integer expected.");
            }
        }

        public static void DisplayOrders(List<Order> orders)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine(string.Format("{0, -3}{1,-40}{2,-10}", i + 1, orders[i].Id, orders[i].MenuItem.Name));
            }
        }

        public static void Refresh()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.Write("\x1b[3j");
        }

        public static void DisplayNotification(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
