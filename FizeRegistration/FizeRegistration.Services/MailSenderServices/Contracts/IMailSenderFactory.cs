namespace FizeRegistration.Services.MailSenderServices.Contracts;

public interface IMailSenderFactory
{
    IMailSenderService NewMailSenderService();
}