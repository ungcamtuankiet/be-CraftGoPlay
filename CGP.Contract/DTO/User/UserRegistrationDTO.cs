using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    /// <summary>
    /// Dữ liệu yêu cầu tạo người dùng.
    /// </summary>
    public class UserRegistrationDTO
    {
        [Required]
        public string UserName { get; set; }    
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        //public IFormFile Thumbnail { get; set; }
        public string PhoneNo { get; set; }

        [Required]
        [PasswordValidation]
        public string PasswordHash { get; set; }
    }
}
