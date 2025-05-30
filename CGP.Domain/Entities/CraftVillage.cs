using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class CraftVillage : BaseEntity
    {
        public string Village_Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
