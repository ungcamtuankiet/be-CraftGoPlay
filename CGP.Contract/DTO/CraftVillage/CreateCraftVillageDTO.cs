using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.CraftVillage
{
    public class CreateCraftVillageDTO
    {
        [Required(ErrorMessage = "Village name is required.")]
        [StringLength(100, ErrorMessage = "Village name cannot exceed 100 characters.")]
        public string Village_Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Established date is required.")]
        public DateTime EstablishedDate { get; set; }
    }
}
