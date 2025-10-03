using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum WalletTransactionTypeEnum
    {
        Purchase,        // Thanh toán đơn hàng (trừ tiền)
        Refund,          // Hoàn tiền đơn hàng (cộng tiền)
        Withdrawal,      // Rút tiền về ngân hàng (trừ tiền)
        ReceiveFromOrder, // Người bán nhận tiền từ đơn hàng (trừ tiền)
        ReceiveShippingFee, // Đơn vị vận chuyển nhận phí ship (trừ tiền)
        SystemAdjustment, // Điều chỉnh số dư bởi hệ thống (dương hoặc âm)
        Pending,        // Giao dịch đang chờ xử lý (ví dụ: thanh toán chưa hoàn tất)
        Release // Giao dịch đã được xử lý và tiền đã được giải phóng (ví dụ: sau khi chờ )
    }
}
