using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Favourite
{
    public class CreateFavouriteDTO
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
