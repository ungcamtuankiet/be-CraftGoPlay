using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public decimal PendingBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public WalletTypeEnum Type { get; set; }
        public Guid User_Id { get; set; }
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
