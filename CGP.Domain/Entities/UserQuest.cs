using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class UserQuest : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid QuestId { get; set; }
        public int Progress { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public bool IsClaimed { get; set; } = false;
        public ApplicationUser User { get; set; }
        public Quest Quest { get; set; }
    }
}
