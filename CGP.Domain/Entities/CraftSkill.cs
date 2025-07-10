using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class CraftSkill : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ApplicationUser> Artisans { get; set; } = new List<ApplicationUser>();
    }
}
