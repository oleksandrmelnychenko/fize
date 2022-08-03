using System.Net.Mail;

namespace FizeRegistration.Client.Helpers.Validation;
public sealed class EmailValidator
{
    public static bool IsEmailValid(string emailAddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailAddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}