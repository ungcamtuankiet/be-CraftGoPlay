using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class UserVoucher : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } 
        public Guid VoucherId { get; set; }
        public Voucher Voucher { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
