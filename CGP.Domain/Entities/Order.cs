using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ArtisanId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
        public ApplicationUser User { get; set; }
        public ApplicationUser Artisan { get; set; }
        public Payment? Payment { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
