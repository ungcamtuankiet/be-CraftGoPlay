using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Order
{
    public class CreateOrderFromCartDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "AddressId is required.")]
        public Guid AddressId { get; set; }

        [Required(ErrorMessage = "DeliveryAmounts is required.")]
        public string DeliveryAmounts { get; set; }
        public decimal Point { get; set; } = 0;
        public string? VoucherProductCode { get; set; }

        public string? VoucherDeliveryCode { get; set; }

        [Required(ErrorMessage = "SelectedCartItemIds is required.")]
        [MinLength(1, ErrorMessage = "SelectedCartItemIds must contain at least one item.")]
        public List<Guid> SelectedCartItemIds { get; set; } = new();

        [Required(ErrorMessage = "PaymentMethod is required.")]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
