using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
namespace Homer_MVC.Controllers
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
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
                var mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(address: "tfn@ytodorov.com", displayName: "YT"));
                // From
                mailMsg.From = new MailAddress(senderMail, senderName);

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;
                mailMsg.Body = emailBody;

                // Init SmtpClient and send
                var smtpClient = new SmtpClient(host: "smtp.sendgrid.net", port: Convert.ToInt32(value: 587));
                //smtpClient.EnableSsl = true;
                var credentials = new System.Net.NetworkCredential(userName: "ytodorov@ytodorov.com", password: "17");
                smtpClient.Credentials = credentials;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMsg);
            }
            return View();
        }
    }
}
