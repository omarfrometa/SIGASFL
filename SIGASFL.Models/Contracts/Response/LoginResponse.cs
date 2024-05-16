using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Contracts.Response
{
    public class LoginResponse
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string? LastIpAddress { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string TwoFactorSecretKey { get; set; }
        public string? Picture { get; set; }
    }
}
