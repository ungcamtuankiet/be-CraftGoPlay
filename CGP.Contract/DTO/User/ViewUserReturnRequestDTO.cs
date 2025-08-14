using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class ViewUserReturnRequestDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Thumbnail { get; set; }
    }
}
