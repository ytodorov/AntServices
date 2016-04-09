﻿using System;
using System.Collections.Generic;
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
                mailMsg.To.Add(new MailAddress("ytodorov@ytodorov.com"));

                // From
                mailMsg.From = new MailAddress("alex_i_bg@mail.bg", "Alex");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Test";


                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("alexxokey", "1510alex");
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
