using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        public Task<List<Inventory>> GetByUserIdAsync(Guid userId);
        public Task<Inventory> GetInventoryByIdAsync(Guid id);
        public Task<Inventory> CheckSlotIndexInventoryAsync(Guid userId, int slotIndex);
        public Task<Inventory> CheckItemInSlotIndexAsync(Guid userId, int slotIndex, string inventoryType);

        public Task<List<Inventory>> GetItemsInInventoryTypeAsync(Guid userId);
        public Task<Inventory> GetItemInInventory(Guid userId, Guid itemId);
    }
}
