using System.Net;
using System.Net.Mail;
using FizeRegistration.Common;
using FizeRegistration.Services.MailSenderServices.Contracts;

namespace FizeRegistration.Services.MailSenderServices;

public class MailSenderFactory : IMailSenderFactory
{
    public IMailSenderService NewMailSenderService()
    {
        return new MailSenderService(
            ConfigurationManager.EmailCredentials.UserName,
            ConfigurationManager.EmailCredentials.Password);
    }
}
