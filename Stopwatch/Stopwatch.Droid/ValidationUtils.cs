
namespace Stopwatch.Core.Utils
{
    public class ValidationUtils
    {
        public static bool LoginValidation(string login)
        {
            return login.Length >=3 ? true : false;
        }

        public static bool PasswordValidation(string password)
        {
            return password.Length >= 3 ? true : false;
        }
    }
}