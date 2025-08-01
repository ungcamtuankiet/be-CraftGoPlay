using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class PointTransaction : BaseEntity
    {
        public Guid Point_Id { get; set; }
        public Point Point { get; set; }
        public decimal Amount { get; set; }
        public PointTransactionEnum Status { get; set; }
        public string? ReferenceCode { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
    }
}
