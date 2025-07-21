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
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
