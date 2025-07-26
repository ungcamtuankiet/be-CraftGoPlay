using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class ReturnRequest : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public ReturnReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } 
        public ReturnStatusEnum Status { get; set; } = ReturnStatusEnum.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime? ApprovedAt { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public bool IsRefunded { get; set; } = false;

        public ApplicationUser User { get; set; }
        public Order Order { get; set; }
    }
}
