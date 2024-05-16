using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Services.Interface
{
    internal interface ISMSService
    {
        bool Send(Models.Twilio settings, string PhoneNumber, string TextMessage);
    }
}
