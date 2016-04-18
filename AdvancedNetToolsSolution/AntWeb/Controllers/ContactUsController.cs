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
        public ActionResult SendEmail(string senderName, string senderMail, string subject, string emailBody)
        {
            if (senderName != "" && senderMail != "" && emailBody != "")
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("tfn@ytodorov.com", "YT"));
                // From
                mailMsg.From = new MailAddress(senderMail, senderName);

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;
                mailMsg.Body = emailBody;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                //smtpClient.EnableSsl = true;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ytodorov@ytodorov.com", "17");
                smtpClient.Credentials = credentials;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMsg);
            }
            return View();
        }
    }
}
