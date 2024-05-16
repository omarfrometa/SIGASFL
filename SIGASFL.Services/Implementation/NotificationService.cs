using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Services.Interface;

namespace SIGASFL.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationConfig config;
        private ISMSService SMSService;
        private IEmailService EmailService;
        public NotificationService(NotificationConfig config)
        {
            this.config = config;

            InitializeSMSService();
        }

        private void InitializeSMSService()
        {
            switch (config.DefaultProviders.EMAIL.ToLower())
            {
                case "office365.com":
                    EmailService = new Office365Service(config.OFFICE365);
                    break;
                case "outlook.com":
                    EmailService = new OutlookService(config.OUTLOOK);
                    break;
                default:
                    EmailService = new Office365Service(config.OFFICE365);
                    break;
            }

            switch (config.DefaultProviders.SMS.ToLower())
            {
                case "twilio":
                    SMSService = new TwilioService(config.Twilio);
                    break;
                default:
                    SMSService = new TwilioService(config.Twilio);
                    break;
            }
        }

        public async Task<ClientResponse<bool>> SendEmail(string ToEmailAddress, string Subject, string HtmlBody)
        {
            var response = new ClientResponse<bool>
            {
                Data = EmailService.Send(ToEmailAddress, Subject, HtmlBody)
            };

            return response;
        }

        public async Task<ClientResponse<bool>> SendSMS(string PhoneNumber, string TextMessage)
        {
            var response = new ClientResponse<bool>
            {
                Data = SMSService.Send(config.Twilio, PhoneNumber, TextMessage)
            };

            return response;
        }
    }
}
