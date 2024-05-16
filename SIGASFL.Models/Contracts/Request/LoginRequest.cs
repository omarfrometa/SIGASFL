using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Contracts.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? LastIpAddress { get; set; } = "127.0.0.1";
    }
}
