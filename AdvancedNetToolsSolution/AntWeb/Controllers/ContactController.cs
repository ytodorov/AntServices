using System;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SmartAdminMvc.Controllers;
using System.Net;

namespace SmartAdminMvc.Controllers
{
    public class ContactController : BaseController
    {
        public ActionResult Index()
        {
            if (HttpContext.Session["random"] == null)
            {
                var r = new Random();
                var a = r.Next(10);
                var b = r.Next(10);
                HttpContext.Session["a"] = a.ToString();
                HttpContext.Session["b"] = b.ToString();
                HttpContext.Session["random"] = (a + b).ToString();
            }
            return View();
        }
        [AcceptVerbs("POST")]

        public ActionResult Index(string name, string email, string comments)
        {
            var result = false;

            ViewData["senderName"] = name;
            ViewData["senderMail"] = email;
            ViewData["comments"] = comments;

            if (HttpContext.Session["random"] != null)   
            {
                var message = new MailMessage(email, "ivanov.alexandar.bg@gmail.com")
                {
                    Subject = "Comment Via Mikesdotnetting from " + name,
                    Body = comments
                };
                message.From = new MailAddress("ivanov.alexandar.bg@gmail.com", "GMail");
               
                message.CC.Add(new MailAddress("ivanov.alexandar.bg@gmail.com"));
                var client = new SmtpClient("smtp.sendgrid.net", 587);
                client.Credentials = new System.Net.NetworkCredential("alexandariv", "Palec!%10");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                try
                {
                    client.Send(message);
                    if (Request.IsAjaxRequest())
                    {
                        return Content(result.ToString());
                    }
                    return result ? View() : View("EmailError");
                }
                catch
                {
                    return View("EmailError");
                }
            }
            return View();
        }
    }
}
