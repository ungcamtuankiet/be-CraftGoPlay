using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Meterial : BaseEntity
    {
        public string MeterialName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
