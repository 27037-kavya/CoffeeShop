namespace CoffeeShop.Validator
{
    internal static class InputValidator
    {
        public static bool IsValiOption<T>(string userInput, out T? option)
            where T : struct, Enum
        {
            if(int.TryParse(userInput, out int userOptionInt) && Enum.IsDefined(typeof(T), userOptionInt))
            {
                option = Enum.Parse<T>(userInput);
                return true;
            }
            option = null;
            return false;
        }
    }
}
