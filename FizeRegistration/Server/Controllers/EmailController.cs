using FizeRegistration.Common;
using Microsoft.AspNetCore.Mvc;

namespace FizeRegistration.Server.Controllers
{
    [Route("Email")]
    [ApiController]
    public class EmailController : Controller
    {
        

        private MailSettings _mailSetting;
        public EmailController(MailSettings mailSetting)
        {
            _mailSetting = mailSetting;
        }

        [HttpPost]
        public IActionResult CreateEmail(MailSettings Email)
        {
            
            MailService mail = new MailService(Email);
            mail.SendEmail();
            return Ok(Email);
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetTables")]
        public IEnumerable<string> Get()
        {
            return  new List<string>() { "biba","boba"};
        }
    }
}
