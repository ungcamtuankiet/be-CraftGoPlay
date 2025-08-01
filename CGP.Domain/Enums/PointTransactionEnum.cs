using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum PointTransactionEnum
    {
        Earned,         // Kiếm điểm (từ mua hàng, hoạt động,...)
        Swap,           // Đổi điểm (thành voucher, quà tặng,...)
        Redeemed,       // Đã sử dụng điểm
        Expired,        // Điểm hết hạn
        Refunded,       // Hoàn lại điểm (khi hủy đơn hàng,...)
        Bonus,          // Điểm thưởng (khuyến mãi, sự kiện)
    }
}
