using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class Quest : BaseEntity
    {
        public string QuestName { get; set; }   
        public string Description { get; set; } 
        public string QuestType { get; set; }
        public string Action { get; set; }
        public string Target { get; set; }
        public string RewardType { get; set; }
        public int RequiredCount { get; set; } = 1;
        public int RewardAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<UserQuest> UserQuests { get; set; } = new List<UserQuest>();
    }
}
