using CGP.Contract.DTO.OrderItem;
using CGP.Contract.DTO.User;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.RefundRequest
{
    public class ViewEscalatedDTO
    {
        public Guid Id { get; set; }
        public ReturnReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public ReturnStatusEnum Status { get; set; }
        public RejectReturnReasonEnum? RejectReturnReasonEnum { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public bool IsRefunded { get; set; }
        public ViewUserReturnRequestDTO User { get; set; }
        public ViewOrderItemDTO OrderItem { get; set; }
    }
}
