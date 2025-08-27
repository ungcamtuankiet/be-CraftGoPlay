using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum WalletTransactionTypeEnum
    {
        TopUp,           // Nạp tiền vào ví (thủ công hoặc cổng VNPAY)
        Purchase,        // Thanh toán đơn hàng (trừ tiền)
        Refund,          // Hoàn tiền đơn hàng (cộng tiền)
        Withdrawal,      // Rút tiền về ngân hàng (trừ tiền)
        ReceiveFromOrder,  // Người bán nhận tiền từ đơn COD (cộng tiền)
        TransferIn,      // Nhận chuyển tiền từ ví khác
        TransferOut,     // Chuyển tiền sang ví khác
        Reward,          // Nhận thưởng (từ hệ thống/game hoặc khuyến mãi)
        Penalty,         // Bị phạt trừ tiền (hoặc hệ thống điều chỉnh)
        SystemAdjustment, // Điều chỉnh số dư bởi hệ thống (dương hoặc âm)
        Pending,        // Giao dịch đang chờ xử lý (ví dụ: thanh toán chưa hoàn tất)
        Release // Giao dịch đã được xử lý và tiền đã được giải phóng (ví dụ: sau khi chờ )
    }
}
