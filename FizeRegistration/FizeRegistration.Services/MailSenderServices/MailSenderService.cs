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

                mssObj.Subject = "Fize sign up confirm";

                var confirmUrl = baseUrl + "/auth/signup/email/confirmation?#access_token=" + tokenData.Token;

                var confirmButton = "<a target=\"_blank\" href=\"" + confirmUrl + "\">link</a>";

                var firstParagraph = "<p>Click the " + confirmButton + " to confirm signing up with Fize!</p>";

                var secondParagraph = "<p style=\"margin-top: 100px;\">Ignore this email if you didn't apply for signing up.</p>";

                var bodyMessage = "<div>" + firstParagraph + secondParagraph + "</div>";

                mssObj.Body = bodyMessage;

                mssObj.IsBodyHtml = true;

                client.Send(mssObj);
            }
        }
        catch
        {
            throw new System.Exception("MailServiceError");
        }
    }
}