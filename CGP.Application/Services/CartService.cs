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
            var getUser = await _unitOfWork.userRepository.GetUserById(userId);
            if(getUser == null)
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            if (cart == null)
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Giỏ hàng không tồn tại",
                    Data = null
                };

            return new Result<CartDto>
            {
                Error = 0,
                Message = "Lấy danh sách sản phầm trong giỏ hàng thành công.",
                Data = _mapper.Map<CartDto>(cart)
            };
        }

        public async Task<Result<CartDto>> AddToCartAsync(Guid userId, AddCartItemDto dto)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            var product = await _unitOfWork.productRepository.GetProductById(dto.ProductId);
            if (product == null)
            {
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }
            var getUser = await _unitOfWork.userRepository.GetUserById(userId);
            if (getUser == null)
            {
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            }
            if (dto.Quantity <= 0)
            {
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Số lượng sản phẩm phải lớn hơn 0.",
                    Data = null
                };
            }

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreationDate = DateTime.UtcNow
                };
                await _unitOfWork.cartRepository.AddCartAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + dto.Quantity > product.Quantity)
                {
                    return new Result<CartDto>
                    {
                        Error = 1,
                        Message = "Số lượng sản phẩm trong giỏ hàng vượt quá số lượng tồn kho.",
                        Data = null
                    };
                }
                existingItem.Quantity += dto.Quantity;
                await _unitOfWork.cartItemRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                if (product.Quantity < dto.Quantity)
                {
                    return new Result<CartDto>
                    {
                        Error = 1,
                        Message = "Số lượng sản phẩm không đủ.",
                        Data = null
                    };
                }
                var item = new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UserId = userId,
                    UnitPrice = product.Price,
                    Cart = cart
                };
                await _unitOfWork.cartItemRepository.AddCartItemAsync(item);
            }

            await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = userId,
                Action = "Thêm sản phẩm vào giỏ hàng.",
                EntityType = "Cart",
                Description = "Bạn đã thêm sản phẩm thành công.",
                EntityId = userId,
            });

            await _unitOfWork.SaveChangeAsync();
            return new Result<CartDto>
            {
                Error = 0,
                Message = "Thêm sản phẩm vào giỏ hàng thành công.",
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
                    Message = "Sản phẩm trong giỏ hàng không tồn tại.",
                    Data = null
                }
            ;
            var getProduct = await _unitOfWork.productRepository.GetProductById(item.ProductId);
            if(dto.Quantity > getProduct.Quantity)
            {
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Số lượng sản phẩm không đủ.",
                    Data = null
                };
            }
            if (dto.Quantity <= 0)
            {
                return new Result<CartDto>
                {
                    Error = 1,
                    Message = "Số lượng sản phẩm phải lớn hơn 0.",
                    Data = null
                };
            }
            item.Quantity = dto.Quantity;
            await _unitOfWork.cartItemRepository.UpdateCartItemAsync(item);
            await _unitOfWork.SaveChangeAsync();

            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(item.Cart.UserId);
            return new Result<CartDto>
            {
                Error = 0,
                Message = "Cập nhật giỏ hàng thành công.",
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
                    Message = "Sản phẩm không tồn tại.",
                    Data = false
                };

            await _unitOfWork.cartItemRepository.RemoveCartItemAsync(item);

            await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = item.UserId,
                Action = "Xóa sản phẩm từ giỏ hàng.",
                EntityType = "Cart",
                Description = "Bạn đã xóa sản phẩm từ giỏ hàng thành công.",
                EntityId = item.UserId,
            });

            await _unitOfWork.SaveChangeAsync();
            return new Result<bool>
            {
                Error = 0,
                Message = "Xóa sản phẩm khỏi giỏ hàng thành công.",
                Data = true
            };
        }
    }
}
