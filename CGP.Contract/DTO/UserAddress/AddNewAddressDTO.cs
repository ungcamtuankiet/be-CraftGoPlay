using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserAddress
{
    public class AddNewAddressDTO
    {
        [Required(ErrorMessage = "User ID là bắt buộc.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Họ và tên phải ít hơn 50 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        public string PhoneNumber { get; set; }
        public TypeAddressEnum AddressType { get; set; }
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Địa phải ít hơn 255 ký tự.")]
        public string FullAddress { get; set; }

        [Required(ErrorMessage = "Vĩ độ là bắt buộc.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Kinh độ là bắt buộc.")]
        public double Longitude { get; set; }
    }
}
