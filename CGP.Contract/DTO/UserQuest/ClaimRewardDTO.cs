using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.UserQuest
{
    public class ClaimRewardDTO
    {
        public Guid UserId { get; set; }
        public Guid UserQuestId { get; set; }
    }
}
