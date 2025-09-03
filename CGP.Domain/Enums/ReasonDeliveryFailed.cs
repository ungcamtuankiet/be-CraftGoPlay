using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum ReasonDeliveryFailed
    {
        Empty,                  // Default value when no reason is provided
        RecipientUnavailable,   // Người nhận không có ở nhà
        WrongAddress,           // Sai/không tìm thấy địa chỉ
        PhoneNotReachable,      // Không liên lạc được số điện thoại
        CustomerRefused,        // Khách từ chối nhận hàng
        DamagedInTransit,       // Hàng bị hư hỏng khi vận chuyển
        Other
    }
}
