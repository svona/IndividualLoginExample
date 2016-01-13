using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Helpers
{
    internal class MyEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MyEmailService.SendMail(message.Destination, message.Subject, message.Body);

            return Task.FromResult(0);
        }

        private static void SendMail(string destination, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                using (var msg = new MailMessage
                {
                    From = new MailAddress("administrator@example.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    msg.To.Add(destination);
                    client.Send(msg);
                }
            }
        }
    }
}