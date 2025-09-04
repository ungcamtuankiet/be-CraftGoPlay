using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly AppDbContext _context;
        public InventoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public Task<Inventory> CheckItemInSlotIndexAsync(Guid userId, int slotIndex, string inventoryType)
        {
            return _context.Inventory
                .Where(i => i.UserId == userId && i.SlotIndex == slotIndex && i.InventoryType == inventoryType)
                .FirstOrDefaultAsync();
        }

        public async Task<Inventory> CheckSlotIndexInventoryAsync(Guid userId, int slotIndex)
        {
            return await _context.Inventory
                .Where(i => i.UserId == userId && i.SlotIndex == slotIndex)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Inventory>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Inventory
                .Include(i => i.User)
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(Guid id)
        {
            return await _context.Inventory
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> GetItemInInventory(Guid userId, Guid itemId)
        {
            return await _context.Inventory
                .FirstOrDefaultAsync(i => i.UserId == userId && i.ItemId == itemId && i.InventoryType == "Backpack");
        }

        public async Task<List<Inventory>> GetItemsInInventoryTypeAsync(Guid userId)
        {
            return await _context.Inventory
                .Where(i => i.UserId == userId && i.InventoryType == "Backpack")
                .ToListAsync();
        }
    }
}
