using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.SubCategory
{
    public class UpdateSubCategoryDTO
    {
        public string? SubName { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        public required CategoryStatusEnum Status { get; set; }
    }
}
