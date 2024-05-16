using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Contracts.Request
{
    public class SecurityOptionRequest
    {
        public int TFAId { get; set; }
        public string UserId { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Question1 { get; set; }
        public string? QuestionCustom1 { get; set; }
        public string? Answer1 { get; set; }
        public int? Question2 { get; set; }
        public string? QuestionCustom2 { get; set; }
        public string? Answer2 { get; set; }
        public int? Question3 { get; set; }
        public string? QuestionCustom3 { get; set; }
        public string? Answer3 { get; set; }
        public List<string> BackupCodes { get; set; } = new List<string>();

        public string? SecurityCode { get; set; }
        public string? SecurityAnswer { get; set; }
    }
}
