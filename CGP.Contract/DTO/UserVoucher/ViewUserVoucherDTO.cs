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
        public Guid UserId { get; set; }
        public List<ViewVoucherDTO> Vouchers { get; set; }
    }
}
