using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public TransactionStatusEnum TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TransactionFee { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Wallet? Wallet { get; set; }
        public Payment Payment { get; set; }
        public Voucher? Voucher { get; set; }
        public ApplicationUser User { get; set; }
    }
}
