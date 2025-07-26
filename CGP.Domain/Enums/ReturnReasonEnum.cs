using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum ReturnReasonEnum
    {
        // Khách hàng đổi ý không muốn mua nữa
        ChangedMind,
        // Nhận sai sản phẩm so với đơn đặt hàng
        WrongItemDelivered,
        // Sản phẩm bị lỗi, hư hỏng
        DamagedOrDefective,
        // Sản phẩm không giống mô tả/ảnh trên website
        NotAsDescribed,
        // Giao hàng trễ hơn thời gian cam kết
        LateDelivery,
        // Sản phẩm không còn nhu cầu sử dụng
        NoLongerNeeded,
        // Thiếu phụ kiện hoặc bộ phận kèm theo
        MissingPartsOrAccessories,
        // Mua nhầm, đặt nhầm sản phẩm
        OrderedByMistake,
        Other
    }
}
