using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;

namespace SIGASFL.Services.Interface
{
    public interface INotificationService
    {
        Task<ClientResponse<bool>> SendEmail(string ToEmailAddress, string Subject, string HtmlBody);
        Task<ClientResponse<bool>> SendSMS(string PhoneNumber, string TextMessage);
    }
}
