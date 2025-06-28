using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Cart;
using CGP.Contract.DTO.CartItem;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CartDto>> ViewCartAsync(Guid userId)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Cart not found.",
                    Data = null
                };

            return new Result<CartDto>
            {
                Error = 0,
                Message = "Cart retrieved successfully.",
                Data = _mapper.Map<CartDto>(cart)
            };
        }

        public async Task<Result<CartDto>> AddToCartAsync(Guid userId, AddCartItemDto dto)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart 
                { 
                    UserId = userId, 
                    CreationDate = DateTime.UtcNow 
                };
                await _unitOfWork.cartRepository.AddCartAsync(cart);
            }

            var product = await _unitOfWork.productRepository.GetProductById(dto.ProductId);
            if (product == null)
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Product not found.",
                    Data = null
                }
            ;

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                await _unitOfWork.cartItemRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var item = new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price,
                    Cart = cart
                };
                await _unitOfWork.cartItemRepository.AddCartItemAsync(item);
            }

            await _unitOfWork.SaveChangeAsync();
            return new Result<CartDto>
            {
                Error = 0,
                Message = "Item added to cart successfully.",
                Data = _mapper.Map<CartDto>(cart)
            };
        }

        public async Task<Result<CartDto>> UpdateCartItemAsync(UpdateCartItemDto dto)
        {
            var item = await _unitOfWork.cartItemRepository.GetCartItemByIdAsync(dto.CartItemId);
            if (item == null)
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Cart item not found.",
                    Data = null
                }
            ;

            item.Quantity = dto.Quantity;
            await _unitOfWork.cartItemRepository.UpdateCartItemAsync(item);
            await _unitOfWork.SaveChangeAsync();

            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(item.Cart.UserId);
            return new Result<CartDto>
            {
                Error = 0,
                Message = "Cart item updated successfully.",
                Data = _mapper.Map<CartDto>(cart)
            };
        }

        public async Task<Result<bool>> RemoveFromCartAsync(Guid cartItemId)
        {
            var item = await _unitOfWork.cartItemRepository.GetCartItemByIdAsync(cartItemId);
            if (item == null)
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Cart item not found.",
                    Data = false
                };

            await _unitOfWork.cartItemRepository.RemoveCartItemAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return new Result<bool>
            {
                Error = 0,
                Message = "Cart item removed successfully.",
                Data = true
            };
        }
    }
}
