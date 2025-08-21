using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Guid Wallet_Id { get; set; }
        public Wallet Wallet { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionTypeEnum Type { get; set; }
        public string? ReferenceCode { get; set; } 
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime? UnlockDate { get; set; }
    }
}
