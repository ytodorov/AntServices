using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Net.Http;
using System.Net.Mime;

namespace SmartAdminMvc.Infrastructure
{
    public class EMailSender
    {
        static void Main()
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("ivanov.alexandar.bg@gmail.com", "Alex"));

                // From
                mailMsg.From = new MailAddress("ivanov.alexandar.bg@gmail.com", "SendGrid");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Test";
                string text = "imame uspeh";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("alexandariv", "Palec!%!0");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

}