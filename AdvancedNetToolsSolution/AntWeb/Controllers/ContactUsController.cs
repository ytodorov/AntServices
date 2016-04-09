using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Homer_MVC.Controllers
{
    public class ContactUsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string name, string email, string title, string emailBody)
        {
            if (name != "" && email != "" && emailBody != "")
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("alex_i_bg@mail.bg", "Alex"));
                // From
                mailMsg.From = new MailAddress(email, name);

                // Subject and multipart/alternative Body
                mailMsg.Subject = title;
                mailMsg.Body = emailBody;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("alexxokey", "1510alex");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            return View();
        }
    }
}
