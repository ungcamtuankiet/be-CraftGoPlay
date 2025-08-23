using CGP.Contract.DTO.Inventory;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<Result<List<ViewInventoryDTO>>> GetByUserIdAsync(Guid userId);
        Task<Result<object>> AddToInventoryAsync(AddToInventoryDTO addToInventoryDTO);
        Task<Result<object>> UpdateInventoryAsync(UpdateInventoryDTO updateInventoryDTO);
        Task<Result<object>> DeleteInventoryAsync(Guid inventoryId);
    }
}
