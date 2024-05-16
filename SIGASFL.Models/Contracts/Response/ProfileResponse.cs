using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Contracts.Response
{
    public class ProfileResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName1 { get; set; }
        public string? LastName2 { get; set; }
        public string? NickName { get; set; }
        public string Gender { get; set; }
    }
}
