using System.Text.RegularExpressions;

namespace FizeRegistration.Client.Helpers.Validation;

public static class PhoneNumber
{
    // Regular expression used to validate a phone number.
    public const string motif = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";

    public static bool IsPhoneNbr(string number)
    {
        if (number != null) return Regex.IsMatch(number, motif);
        else return false;
    }
}