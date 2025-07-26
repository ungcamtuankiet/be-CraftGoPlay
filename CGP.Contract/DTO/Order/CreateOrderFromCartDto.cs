using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class CreateOrderFromCartDto
    {
        public Guid UserId { get; set; } 
        public Guid AddressId { get; set; }
        public double DeliveryAmount { get; set; }
        public List<Guid> SelectedCartItemIds { get; set; } = new();
        public PaymentMethodEnum PaymentMethod { get; set; } 
    }
}
