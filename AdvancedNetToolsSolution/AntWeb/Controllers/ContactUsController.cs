using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency

namespace Homer_MVC.Controllers
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
{
    public class ContactUsController : Controller
    {
        [OutputCache(CacheProfile = "MyCache")]
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
                    var myMessage = new SendGrid.SendGridMessage();
                    myMessage.AddTo("ToolsForNet@ytodorov.com");
                    myMessage.From = new MailAddress(senderMail, senderName);
                    myMessage.Subject = subject;
                    myMessage.Text = emailBody;

                    var apiKey = ConfigurationManager.AppSettings["sendgridApiKey"];

                    var transportWeb = new SendGrid.Web(apiKey);

                    transportWeb.DeliverAsync(myMessage);
                }
                catch (Exception ex)
                {
                    var result = new { error = ex.Message };
                    return Json(result);
                }
            }
            return new EmptyResult();
        }
    }
}