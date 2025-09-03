using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserVoucher
{
    public class GetAllVouchersByUserIdDTO
    {
        public Guid UserId { get; set; }
        public VoucherTypeEnum VoucherType { get; set; }
    }
}
