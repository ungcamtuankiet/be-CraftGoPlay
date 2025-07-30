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
        public Guid? UserAddressId { get; set; }
        public double Product_Amount { get; set; }
        public double Delivery_Amount { get; set; }
        public decimal TotalPrice { get; set; } 
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Created;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
        public ApplicationUser User { get; set; }
        public Payment? Payment { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<OrderVoucher> OrderVouchers { get; set; }
        [ForeignKey("TransactionId")]
        public ICollection<Transaction> Transactions { get; set; }
        [ForeignKey("UserAddressId")]   
        public UserAddress? UserAddress { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }

        public static implicit operator string(Order v)
        {
            throw new NotImplementedException();
        }
    }
}
