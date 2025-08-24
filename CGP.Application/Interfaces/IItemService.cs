using CGP.Contract.DTO.Item;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IItemService
    {
        public Task<Result<object>> GetAllItemsAsync();
        public Task<Result<object>> GetItemByIdAsync(Guid id);
        public Task<Result<object>> CreateItem(CreateItemDTO createItemDTO);
        public Task<Result<object>> UpdateItem(UpdateItemDTO updateItemDTO);
        public Task<Result<object>> DeleteItem(Guid id);
    }
}
