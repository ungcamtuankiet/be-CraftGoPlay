using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserVoucher
{
    public class SwapVoucherDTO
    {
        public Guid UserId { get; set; }
        public Guid VoucherId { get; set; }
    }
}
