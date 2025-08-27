using CGP.Contract.DTO.WalletTransaction;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Wallet
{
    public class ViewWalletDTO
    {
        public Guid Id { get; set; }
        public Guid User_Id { get; set; }
        public WalletTypeEnum Type { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public List<ViewWalletTransactionDTO> WalletTransactions { get; set; }
    }
}
