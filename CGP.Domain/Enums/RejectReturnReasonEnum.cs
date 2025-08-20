using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum RejectReturnReasonEnum
    {
        ProductNotDefective,     // Sản phẩm không bị lỗi như khách báo
        WrongUsage,              // Khách sử dụng sai cách gây hỏng
        MissingEvidence,         // Khách không cung cấp đủ bằng chứng
        ExceededReturnTime,      // Quá hạn thời gian cho phép hoàn trả
        ProductUsedOrDamaged,    // Sản phẩm đã qua sử dụng hoặc hư hỏng do khách
        NotMatchReturnPolicy,    // Không đúng chính sách hoàn trả
        AccessoriesMissing,      // Thiếu phụ kiện / quà tặng kèm
        WrongItemReturned,       // Khách trả nhầm sản phẩm không thuộc đơn hàng
        PackagingNotOriginal,    // Bao bì, tem niêm phong không còn nguyên vẹn
        Other                    // Lý do khác (shop tự nhập)
    }
}
