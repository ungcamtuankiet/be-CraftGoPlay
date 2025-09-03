using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Enums
{
    public enum OrderStatusEnum
    {
        Created,             // Đơn hàng vừa được tạo, chưa được xử lý
        Confirmed,           // Đơn hàng đã được artisan xác nhận
        Rejected,            // Đơn hàng bị artisan từ chối
        Preparing,           // Đơn hàng đang được artisan chuẩn bị
        AwaitingPayment,     // Đơn hàng chờ thanh toán (cho thanh toán online)
        PaymentFailed,       // Thanh toán thất bại (cho thanh toán online)
        ReadyForShipment,    // Đơn hàng đã sẵn sàng để giao
        Shipped,             // Đơn hàng đã được giao cho đơn vị vận chuyển
        Delivered,           // Đơn hàng đã được giao đến người dùng
        Completed,           // Đơn hàng hoàn thành (người dùng xác nhận hoặc tự động hoàn thành)
        Cancelled,           // Đơn hàng bị hủy (bởi người dùng, artisan, hoặc hệ thống)
        ReturnRequested,     // Người dùng yêu cầu trả hàng
        Returned,            // Đơn hàng đã được trả lại
        PartialReturn,       // Hoàn trả 1 phần
        FullReturn,          // Hoàn trả toàn phần
        Refunded,            // Đơn hàng đã được hoàn tiền
        DeliveryAttemptFailed, // Thử giao hàng không thành công (người dùng không có mặt)
        DeliveryFailed       // Giao hàng thất bại (địa chỉ sai, không liên lạc được)
    }
}
