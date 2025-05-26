using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Category
{
    public class ViewSubCateDTO
    {
        public required string SubId { get; set; }
        public string? SubName { get; set; }
        public required int Status { get; set; }
        public required Guid CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
