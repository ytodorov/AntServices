using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
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
                try
                {
                    var mailMsg = new MailMessage();

                    // To
                    mailMsg.To.Add(new MailAddress(address: "EmailFromToolsfornet@ytodorov.com"));

                    // From
                    mailMsg.From = new MailAddress(address: senderMail, displayName: senderName);
                    string html = emailBody;
                    // Subject and multipart/alternative Body
                    mailMsg.Subject = subject;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html,
                        contentEncoding: null, mediaType: MediaTypeNames.Text.Html));

                    // Init SmtpClient and send
                    var smtpClient = new SmtpClient(host: "smtp.sendgrid.net", port: Convert.ToInt32(value: 587));
                    string userName = ConfigurationManager.AppSettings["smtpUserName"];
                    string password = ConfigurationManager.AppSettings["smtpPassword"];
                    var credentials = new System.Net.NetworkCredential(userName, password);
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    
                    var result = new { error = "Error sending mail!" };
                    return Json(result);
                }
            }
            return new EmptyResult();
        }
    }
}
