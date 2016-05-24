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
                var mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(address: "test@ytodorov.com"));

                // From
                mailMsg.From = new MailAddress(address: "ytodorov@ytodorov.com", displayName: "Dancho");
                string html = @"<p>html body</p>";
                // Subject and multipart/alternative Body
                mailMsg.Subject = "Test";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, contentEncoding: null, mediaType: MediaTypeNames.Text.Html));
                
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
                Console.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void SendMailSendGridLibraryTest()
        {

            try
            {
                var myMessage = new SendGrid.SendGridMessage();
                myMessage.AddTo("test@ytodorov.com");
                myMessage.From = new MailAddress("test@youremail.com", "First Last");
                myMessage.Subject = "Sending with SendGrid is Fun";
                myMessage.Text = "and easy to do anywhere, even with C#";

                var transportWeb = new SendGrid.Web("api key");
                transportWeb.DeliverAsync(myMessage).Wait();
                 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
