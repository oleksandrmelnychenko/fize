using System.Net;
using System.Net.Mail;
using FizeRegistration.Services.MailSenderServices.Contracts;
using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Services.MailSenderServices;

public class MailSenderService : IMailSenderService
{
    private readonly string _senderUserName;

    private readonly string _senderPassword;

    public MailSenderService(string senderUserName, string senderPassword)
    {
        _senderUserName = senderUserName;
        _senderPassword = senderPassword;
    }

    public void SendTokenToEmail(string email, TokenDataContract tokenData, string baseUrl)
    {
        try
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(_senderUserName, _senderPassword);

                MailMessage mssObj = new MailMessage();

                mssObj.To.Add(email);

                mssObj.From = new MailAddress(_senderUserName);

                mssObj.Subject = "TextApp sign up confirm";

                mssObj.Body = baseUrl + "create?#access_token=" + tokenData.Token;

                client.Send(mssObj);
            }
        }
        catch
        {
            throw new System.Exception("MailServiceError");
        }
    }
}