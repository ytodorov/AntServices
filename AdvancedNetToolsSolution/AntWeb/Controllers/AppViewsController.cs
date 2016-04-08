using System;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SmartAdminMvc.Controllers;
using System.Net;

namespace MikesDotnetting.Controllers
{
    public class AppViewsController : BaseController
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

        public ActionResult Index(string name, string email, string comments, string preventspam)
        {
            const string emailregex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            var result = false;

            ViewData["name"] = name;
            ViewData["email"] = email;
            ViewData["comments"] = comments;
            ViewData["preventspam"] = preventspam;

            if (string.IsNullOrEmpty(name))
                ViewData.ModelState.AddModelError("name", "Please enter your name!");
            if (string.IsNullOrEmpty(email))
                ViewData.ModelState.AddModelError("email", "Please enter your e-mail!");
            if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, emailregex))
                ViewData.ModelState.AddModelError("email", "Please enter a valid e-mail!");
            if (string.IsNullOrEmpty(comments))
                ViewData.ModelState.AddModelError("comments", "Please enter a message!");
            if (string.IsNullOrEmpty(preventspam))
                ViewData.ModelState.AddModelError("preventspam", "Please enter the total");
            if (!ViewData.ModelState.IsValid)
                return View();

            if (HttpContext.Session["random"] != null &&
              preventspam == HttpContext.Session["random"].ToString())
            {
                var message = new MailMessage(email, "ivanov.alexandar.bg@gmail.com")
                {
                    Subject = "Comment Via Mikesdotnetting from " + name,
                    Body = comments
                };

                var client = new SmtpClient("smtp.sendgrid.net", 587);
                try
                {
                    client.Send(message);
                    result = true;
                }
                catch
                {
                }
            }
            if (Request.IsAjaxRequest())
            {
                return Content(result.ToString());
            }
            return result ? View() : View("EmailError");
        }
    }
}
