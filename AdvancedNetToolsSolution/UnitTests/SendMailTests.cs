using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class SendMailTests
    {
        [Fact]
        public void SendMailTest()
        {

            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("test@ytodorov.com"));

                // From
                mailMsg.From = new MailAddress("ytodorov@ytodorov.com", "Dancho");
                string html = @"<p>html body</p>";
                // Subject and multipart/alternative Body
                mailMsg.Subject = "Test";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                
                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                string userName = ConfigurationManager.AppSettings["smtpUserName"];
                string password = ConfigurationManager.AppSettings["smtpPassword"];
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userName, password);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);

                //SendGrid.SendGridMessage sg;
                //sg.EnableTemplate();
                //sg.EnableTemplateEngine()

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
