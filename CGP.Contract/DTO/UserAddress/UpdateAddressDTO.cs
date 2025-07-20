using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserAddress
{
    public class UpdateAddressDTO
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

        [Required(ErrorMessage = "Mã tỉnh(Thành phố) là bắt buộc.")]
        public int ProviceId { get; set; }

        [Required(ErrorMessage = "Tên tỉnh(Thành phố) là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên tỉnh(Thành phố) phải ít hơn 50 ký tự.")]
        public string ProviceName { get; set; }

        [Required(ErrorMessage = "Mã quận(huyện) là bắt buộc.")]
        public int DistrictId { get; set; } 

        [Required(ErrorMessage = "Tên quận(huyện) là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên quận(huyện) phải ít hơn 50 ký tự.")]
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Mã xã(phường) là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Mã xã(phường) phải ít hơn 50 ký tự.")]
        public string WardCode { get; set; }

        [Required(ErrorMessage = "Tên xã(phường) là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên xã(phường) phải ít hơn 50 ký tự.")]
        public string WardName { get; set; }

        [Required(ErrorMessage = "Số nhà và tên đường là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Số nhà và tên đường phải ít hơn 50 ký tự.")]
        public string HomeNumber { get; set; }
    }
}
