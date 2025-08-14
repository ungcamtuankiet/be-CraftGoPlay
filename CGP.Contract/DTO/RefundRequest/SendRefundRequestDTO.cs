using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.RefundRequest
{
    public class SendRefundRequestDTO
    {
        public Guid OrderItemId { get; set; }
        public Guid UserId { get; set; }
        public ReturnReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
