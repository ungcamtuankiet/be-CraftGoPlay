using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.WalletTransaction
{
    public class ViewWalletTransactionDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionTypeEnum Type { get; set; }
        public string? Description { get; set; }
        public DateTime DateTransaction { get; set; }
    }
}
