using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Voucher : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VoucherTypeEnum Type { get; set; }
        public VoucherDiscountTypeEnum DiscountType { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public double MinOrderValue { get; set; }
        public double MaxDiscountAmount { get; set; }
        public double PointChangeAmount { get; set; }
        public int UsedCount { get; set; } = 0;
        public int ChangeAmout { get; set; } = 0;
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<OrderVoucher> OrderVouchers { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
