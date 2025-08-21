using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Rating
{
    public class CheckedRatingDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid OrderItemId { get; set; }
    }
}
