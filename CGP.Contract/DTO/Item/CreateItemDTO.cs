using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Item
{
    public class CreateItemDTO
    {
        public Guid Id { get; set; }
        public string NameItem { get; set; }
        public string Description { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public bool IsStackable { get; set; }
    }
}
