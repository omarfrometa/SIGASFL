using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Contracts.Request
{
    public class PWDRequest
    {
        public string? UserId { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
