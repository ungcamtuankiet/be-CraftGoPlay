using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.ActivityLog
{
    public class AddActivityDTO
    {
        public Guid? UserId { get; set; }
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public Guid? EntityId { get; set; } // ID thực thể liên quan (vd: OrderId)
        public string Description { get; set; } = null!;
    }
}
