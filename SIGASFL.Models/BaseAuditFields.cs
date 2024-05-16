using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models
{
    public class BaseAuditFields<T> : BaseView<T>
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
