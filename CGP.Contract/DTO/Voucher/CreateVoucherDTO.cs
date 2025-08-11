using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Voucher
{
    public class CreateVoucherDTO
    {
        [Required(ErrorMessage = "Mã giảm giá là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Mã giảm giá phải ít hơn 50 ký tự.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Tên phiếu giảm giá là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên phiếu giảm giá phải ít hơn 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả phiếu giảm giá là bắt buộc.")]
        [MaxLength(250, ErrorMessage = "Mô tả phiếu giảm giá phải ít hơn 250 ký tự.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Loại phiếu giảm giá là bắt buộc.")]
        public VoucherTypeEnum Type { get; set; }

        [Required(ErrorMessage = "Loại giảm giá là bắt buộc.")]
        public VoucherDiscountTypeEnum DiscountType { get; set; }

        [Required(ErrorMessage = "Loại thanh toán cho phương giảm giá là bắt buộc.")]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị nhỏ nhất để sử dụng mã giảm giá phải lớn hơn hoặc bằng 0.")]
        public double MinOrderValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị lớn nhất để sử dụng mã giảm giá phải lớn hơn hoặc bằng 0.")]
        public double MaxDiscountAmount { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Giảm giá là bắt buộc.")]
        public double Discount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
