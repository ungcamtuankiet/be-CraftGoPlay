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
    public class ShopPriceRepository : GenericRepository<ShopPrice>, IShopPriceRepository
    {
        private readonly AppDbContext _context;
        public ShopPriceRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<ShopPrice>> GetAllShopPrice()
        {
            return await _context.ShopPrice
                .Include(sp => sp.Item)
                .ToListAsync();
        }

        public async Task<ShopPrice> GetItemInShop(Guid itemId)
        {
            return await _context.ShopPrice
                .FirstOrDefaultAsync(sp => sp.ItemId == itemId);
        }

        public async Task<ShopPrice> GetShopPriceById(Guid id)
        {
            return await _context.ShopPrice
                .Include (sp => sp.Item)
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }
    }
}
