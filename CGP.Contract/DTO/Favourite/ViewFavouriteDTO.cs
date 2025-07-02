using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Favourite
{
    public class ViewFavouriteDTO
    {
        public Guid Id { get; set; }
        public ViewProductFavouriteDTO Product { get; set; }
    }
}
