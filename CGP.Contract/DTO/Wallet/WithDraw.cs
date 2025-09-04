using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Wallet
{
    public class WithDraw
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string BankCode { get; set; } 
        public string BankAccount { get; set; }
    }
}
