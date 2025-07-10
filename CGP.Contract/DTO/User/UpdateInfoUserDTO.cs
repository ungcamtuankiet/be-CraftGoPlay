using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class UpdateInfoUserDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
