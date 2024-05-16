using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models.Views
{
    public class TwoFactorAuthenticatorView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public short? OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
