using CGP.Domain.Enums;
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
        public QuestType QuestType { get; set; }
        public string Reward { get; set; }
        public bool IsDaily { get; set; }
        public ICollection<UserQuest> UserQuests { get; set; } = new List<UserQuest>();
    }
}
