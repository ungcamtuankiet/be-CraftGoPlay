using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Auth
{
    public class Authenticator
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
