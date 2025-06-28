using CGP.Contract.DTO.Cart;
using CGP.Contract.DTO.CartItem;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ICartService
    {
        Task<Result<CartDto>> ViewCartAsync(Guid userId);
        Task<Result<CartDto>> AddToCartAsync(Guid userId, AddCartItemDto dto);
        Task<Result<CartDto>> UpdateCartItemAsync(UpdateCartItemDto dto);
        Task<Result<bool>> RemoveFromCartAsync(Guid cartItemId);
    }
}
