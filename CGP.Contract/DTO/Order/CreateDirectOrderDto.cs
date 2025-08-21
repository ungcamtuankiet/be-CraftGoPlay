using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class CreateDirectOrderDto
    {
        public Guid ProductId { get; set; }
        public Guid AddressId { get; set; }
        public double DeliveryAmount { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public string? VoucherProductCode { get; set; }
        public string? VoucherDeliveryCode { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
