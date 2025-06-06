﻿using CGP.Contract.DTO.SubCategory;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Category
{
    public class CreateCategoryWithSubDTO
    {
        public CreateCategoryWithSubDTO()
        {
            SubCategories = new HashSet<CreateSubCategoryDTO>();
        }
        public required string CategoryName { get; set; }
        public required CategoryStatusEnum CategoryStatus { get; set; }
        public virtual ICollection<CreateSubCategoryDTO> SubCategories { get; set; }
    }
}
