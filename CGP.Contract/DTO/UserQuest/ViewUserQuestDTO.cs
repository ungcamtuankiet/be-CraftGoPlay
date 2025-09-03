using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserQuest
{
    public class ViewUserQuestDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestId { get; set; }
        public int Progress { get; set; } 
        public QuestStatus Status { get; set; }
        public bool RewardClaimed { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
