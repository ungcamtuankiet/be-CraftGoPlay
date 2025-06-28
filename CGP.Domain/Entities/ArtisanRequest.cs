using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Domain.Entities
{
    public class ArtisanRequest : BaseEntity
    {
        public string Image { get; set; }
        public Guid CraftVillageId { get; set; }
        public Guid UserId { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public string? Reason { get; set; }
        public RequestArtisanStatus Status { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CraftVillageId")]
        public CraftVillage CraftVillages { get; set; }
    }
}
