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
        public string Name { get; set; }
        public float Balance { get; set; }
        public Guid User_Id { get; set; }
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }
    }
}
