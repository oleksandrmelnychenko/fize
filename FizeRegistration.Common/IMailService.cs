namespace FizeRegistration.Common
{
    public interface IMailService
    {
        Task SendEmail();
        bool IsValidEmail(string email);
    }
}