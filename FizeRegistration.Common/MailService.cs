using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FizeRegistration.Common
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailConfig;
        public MailService(MailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }
        //irabalicka77@gmail.com
        public async Task Send1Email()
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("natusvincer77@gmail.com");
            message.To.Add(new MailAddress(_mailConfig.ToEmail));
            message.Subject = "Test";
            message.IsBodyHtml = true;
            message.Body = "https://localhost:7036/";
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("natusvincer77@gmail.com", "okkkmiqgurdpqqnw");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
        public async Task SendEmail()
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("natusvincer77@gmail.com", "okkkmiqgurdpqqnw");
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(_mailConfig.ToEmail));
                message.From = new MailAddress("natusvincer77@gmail.com");
                message.Subject = "Test";
                message.Body = "ишиф";
                await client.SendMailAsync(message);
            }
        }

        public  bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}