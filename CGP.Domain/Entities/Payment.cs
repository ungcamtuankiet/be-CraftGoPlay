using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public string TransactionNo { get; set; }
        public string BankCode { get; set; }
        public string ResponseCode { get; set; } 
        public string SecureHash { get; set; }
        public string RawData { get; set; } 
        public bool IsSuccess => ResponseCode == "00";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
