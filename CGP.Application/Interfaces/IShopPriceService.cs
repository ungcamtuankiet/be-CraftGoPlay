using CGP.Contract.DTO.ShopPrice;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IShopPriceService
    {
        Task<Result<object>> GetAllItemShopPrice();
        Task<Result<object>> GetItemShopPrice(Guid shopId);
        Task<Result<object>> CreateShopPriceItem(CreateItemShopPriceDTO dto);
        Task<Result<object>> UpdateShopPriceItem(UpdateItemShopPriceDTO dto);
        Task<Result<object>> RemoveShopPriceItem(Guid id);
    }
}
