using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models
{
    public class NotificationConfig
    {
        public Twilio Twilio { get; set; }
        public OUTLOOK OUTLOOK { get; set; }
        public OFFICE365 OFFICE365 { get; set; }
        public DefaultProviders DefaultProviders { get; set; }
    }

    public class Twilio
    {
        public string ACCOUNTSID { get; set; }
        public string AUTHTOKEN { get; set; }
        public string NUMBER { get; set; }
    }

    public class OUTLOOK
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class OFFICE365
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool UseBasicCredentials { get; set; }
        public Credentials Credentials { get; set; }
        public bool EnableSsl { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class DefaultProviders
    {
        public string EMAIL { get; set; }
        public string SMS { get; set; }
    }
}
