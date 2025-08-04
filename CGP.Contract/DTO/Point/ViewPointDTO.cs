using CGP.Contract.DTO.PointTransaction;
using CGP.Contract.DTO.User;
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
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ViewPointTransactionDTO> PointTransactions { get; set; }
    }
}
