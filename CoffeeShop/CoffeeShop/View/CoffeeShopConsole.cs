using CoffeeShop.Models;
using CoffeeShop.Validator;
using System.Text;
using System.Text.RegularExpressions;

namespace CoffeeShop.View
{
    internal static class CoffeeShopConsole
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

        public static  T GetOption<T>()
            where T: struct, Enum
        {
            DisplayMenu<T>();
            T? validatedOption;
            do
            {
                string userInput = Console.ReadLine()?.Trim() ?? string.Empty;

                if (!InputValidator.IsValiOption<T>(userInput, out validatedOption))
                {
                    Console.WriteLine("Invalid option.");
                }
                return validatedOption ?? default;
            } while (true);
        }

        public static void DisplayMenuItems(List<MenuItem> menu)
        {
           for(int i=0;i<menu.Count;i++)
            {
                Console.WriteLine($"{i+1}. {menu[i]}");
            }
        }

        public static int GetMenuOption()
        {
            string userOption = Console.ReadLine()?.Trim() ?? string.Empty;
            while (true)
            {
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
    }
}
