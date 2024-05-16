using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Services.Interface
{
    public interface IEmailService
    {
        bool Send(string ToEmailAddress, string Subject, string HtmlBody);
    }
}
