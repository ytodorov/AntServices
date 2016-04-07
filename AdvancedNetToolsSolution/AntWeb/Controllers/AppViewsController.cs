#region Using

using System;
using System.Net.Mail;
using System.Web.Mvc;
using SmartAdminMvc.Models;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace SmartAdminMvc.Controllers
{
    
    public class AppViewsController : Controller
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailCompose(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "Palec";
                var message = new MailMessage();
                message.To.Add(new MailAddress("ivanov.alexandar.bg@gmil.com"));  // replace with valid value 
                message.From = new MailAddress("ivanov.alexandar.bg@gmil.com");  // replace with valid value
                message.Subject = "Test";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "ivalexandar",  // replace with valid value
                        Password = "Palec!%!0"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.sendgrid.net";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("EmailCompose");
                }
            }
            return View(model);
        }
        public ActionResult EmailCompose()
        {
            return View();
        }
    }
}