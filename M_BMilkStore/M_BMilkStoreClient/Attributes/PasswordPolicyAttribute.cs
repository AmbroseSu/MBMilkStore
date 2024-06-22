using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace M_BMilkStoreClient.Attributes
{
    public class PasswordPolicyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (password == null)
            {
                return false;
            }

            var hasUpperCaseLetter = new Regex(@"[A-Z]+").IsMatch(password);
            var hasSpecialCharacter = new Regex(@"[\W_]+").IsMatch(password);

            return hasUpperCaseLetter && hasSpecialCharacter;
        }

        public override string FormatErrorMessage(string name)
        {
            return "The password must contain at least one uppercase letter and one special character.";
        }
    }
}
