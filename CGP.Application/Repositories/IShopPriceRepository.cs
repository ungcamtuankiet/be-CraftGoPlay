using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IShopPriceRepository : IGenericRepository<ShopPrice>
    {
        Task<List<ShopPrice>> GetAllShopPrice();
        Task<ShopPrice> GetShopPriceById(Guid id);
        Task<ShopPrice> GetItemInShop(Guid itemId);
    }
}
