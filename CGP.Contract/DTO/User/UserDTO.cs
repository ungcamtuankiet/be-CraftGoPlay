using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }

        public double? Balance { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
