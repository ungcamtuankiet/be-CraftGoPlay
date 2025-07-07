using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class CreateNewAccountDTO
    {
        [Required(ErrorMessage = "UserName là bắt buộc.")]
        [StringLength(50, ErrorMessage = "UserName không được vượt quá 50 ký tự.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 20 ký tự.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,20}$",ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Ngày sinh phải đúng định dạng yyyy-MM-dd.")]
        [SwaggerSchema(Description = "Ngày sinh định dạng yyyy-MM-dd")]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(10, ErrorMessage = "Số điện thoại không được dài quá 20 ký tự.")]
        public string? PhoneNumber { get; set; }
        public IFormFile? Thumbnail { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [EnumDataType(typeof(StatusEnum), ErrorMessage = "Trạng thái không hợp lệ.")]
        public StatusEnum Status { get; set; } = StatusEnum.Active;

        [Required(ErrorMessage = "Quyền (Role) là bắt buộc.")]
        [EnumDataType(typeof(RoleEnum), ErrorMessage = "Role không hợp lệ.")]
        public RoleEnum RoleId { get; set; }
    }
}
