using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Services.Interface;

namespace SIGASFL.Services.Implementation
{
    internal class Office365Service : IEmailService
    {
        private readonly OFFICE365 config;
        public Office365Service(OFFICE365 config)
        {
            this.config = config;
        }
        public bool Send(string ToEmailAddress, string Subject, string HtmlBody)
        {
            bool result = false;
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(config.Credentials.Username, Helpers.Utils.ToSecureString(config.Credentials.Password));
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(config.From);

                    smtpClient.Host = config.Host;
                    smtpClient.Port = config.Port;
                    smtpClient.UseDefaultCredentials = config.UseDefaultCredentials;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = config.EnableSsl;
                    message.From = fromAddress;
                    message.Subject = Subject;
                    message.IsBodyHtml = true;
                    message.Body = HtmlBody;
                    message.To.Add(ToEmailAddress);

                    try
                    {
                        smtpClient.Send(message);
                        result = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }
    }
}
