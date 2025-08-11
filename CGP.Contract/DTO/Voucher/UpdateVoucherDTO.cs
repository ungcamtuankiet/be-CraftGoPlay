using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Voucher
{
    public class UpdateVoucherDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VoucherTypeEnum Type { get; set; }
        public VoucherDiscountTypeEnum DiscountType { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public double MinOrderValue { get; set; }
        public double MaxDiscountAmount { get; set; }
        public int UsedCount { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
