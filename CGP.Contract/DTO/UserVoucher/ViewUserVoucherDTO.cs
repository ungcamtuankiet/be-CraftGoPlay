using CGP.Contract.DTO.Voucher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserVoucher
{
    public class ViewUserVoucherDTO
    {
        public ViewVoucherDTO Voucher { get; set; }
        public bool IsUsed { get; set; }    
    }
}
