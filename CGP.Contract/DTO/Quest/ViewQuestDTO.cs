using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Quest
{
    public class ViewQuestDTO
    {
        public Guid Id { get; set; }
        public string QuestName { get; set; }
        public string Description { get; set; }
        public QuestType QuestType { get; set; }
        public string Reward { get; set; }
        public int AmountReward { get; set; }
        public bool IsDaily { get; set; }
    }
}
