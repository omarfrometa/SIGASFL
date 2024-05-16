using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Services.Interface;

namespace SIGASFL.Services.Implementation
{
    internal class TwilioService : ISMSService
    {
        private readonly Models.Twilio twilioConfi;
        public TwilioService(Models.Twilio twilioConfig)
        {
            this.twilioConfi = twilioConfig;
        }
        public bool Send(Models.Twilio settings, string PhoneNumber, string TextMessage)
        {
            bool result = false;

            try
            {
                /*TwilioClient.Init(settings.ACCOUNTSID, settings.AUTHTOKEN);

                var response = MessageResource.Create(
                    body: TextMessage,
                    from: new Twilio.Types.PhoneNumber(settings.NUMBER),
                    to: new Twilio.Types.PhoneNumber(PhoneNumber)
                );*/

                result = true;
            }
            catch { }

            return result;
        }
    }
}
