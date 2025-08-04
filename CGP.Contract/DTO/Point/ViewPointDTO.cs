using CGP.Contract.DTO.PointTransaction;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Point
{
    public class ViewPointDTO
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; } = 0;
        public ApplicationUser User { get; set; }
        public ViewPointTransactionDTO PointTransactions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
