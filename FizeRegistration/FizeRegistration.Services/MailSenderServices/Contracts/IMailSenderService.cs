using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Services.MailSenderServices.Contracts;

public interface IMailSenderService
{
    void SendTokenToEmail(string email, TokenDataContract tokenData, string baseUrl);
}