using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TransactionId { get; set; }
        public double Product_Amount { get; set; }
        public double Delivery_Amount { get; set; }
        public double ProductDiscount { get; set; }
        public double DeliveryDiscount { get; set; }
        public double TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public int DeliveriesCount { get; set; } = 0;
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Created;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public ReasonDeliveryFailed ReasonDeliveryFailed { get; set; } = ReasonDeliveryFailed.Empty;
        public bool IsPaid { get; set; } = false;
        public ApplicationUser User { get; set; }
        public Payment? Payment { get; set; }
        public OrderAddress OrderAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<OrderVoucher> OrderVouchers { get; set; }
        [ForeignKey("TransactionId")]
        public ICollection<Transaction> Transactions { get; set; }

        public static implicit operator string(Order v)
        {
            throw new NotImplementedException();
        }
    }
}
