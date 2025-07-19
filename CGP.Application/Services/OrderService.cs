using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Order;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayoutService _payoutService;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public OrderService(IUnitOfWork unitOfWork, IPayoutService payoutService, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _payoutService = payoutService;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<Result<List<ViewOrderDTO>>> GetOrdersAsync()
        {
            var orders = await _unitOfWork.orderRepository.GetListOrderAsync();
            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders);

            return new Result<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng thành công",
                Data = orderDtos
            };
        }

        public async Task<Result<ViewOrderDTO>> GetOrderByIdAssync(Guid id)
        {
            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return new Result<ViewOrderDTO>()
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại",
                    Data = null
                };
            }

            var currentUserId = _claimsService.GetCurrentUserId;
            // Kiểm tra quyền sở hữu
            if (order.UserId != currentUserId)
            {
                return new Result<ViewOrderDTO>()
                {
                    Error = 2,
                    Message = "Bạn không có quyền truy cập đơn hàng này",
                    Data = null
                };
            }

            var orderDto = _mapper.Map<ViewOrderDTO>(order); // Use AutoMapper
            return new Result<ViewOrderDTO>()
            {
                Error = 0,
                Message = "Lấy thông tin đơn hàng thành công",
                Data = orderDto
            };
        }

        public async Task<Result<List<ViewOrderDTO>>> GetOrdersByUserIdAsync(Guid userId)
        {
            var currentUserId = _claimsService.GetCurrentUserId;

            // Kiểm tra quyền: Chỉ cho phép người dùng xem đơn hàng của chính họ
            if (userId != currentUserId)
            {
                return new Result<List<ViewOrderDTO>>()
                {
                    Error = 2,
                    Message = "Bạn không có quyền xem đơn hàng của người dùng này",
                    Data = null
                };
            }

            var orders = await _unitOfWork.orderRepository.GetOrdersByUserIdAsync(userId);
            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders); // Use AutoMapper

            return new Result<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng theo người dùng thành công",
                Data = orderDtos
            };
        }

        public async Task<Result<List<ViewOrderDTO>>> GetOrdersByArtisanIdAsync(Guid artisanId)
        {
            var currentArtisanId = _claimsService.GetCurrentUserId;

            // Kiểm tra quyền: Chỉ cho phép người dùng xem đơn hàng của chính họ
            if (artisanId != currentArtisanId)
            {
                return new Result<List<ViewOrderDTO>>()
                {
                    Error = 2,
                    Message = "Bạn không có quyền xem đơn hàng của người dùng này",
                    Data = null
                };
            }

            var orders = await _unitOfWork.orderRepository.GetOrdersByArtisanIdAsync(artisanId);
            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders); // Use AutoMapper

            return new Result<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng theo artisan thành công",
                Data = orderDtos
            };
        }

        public async Task<Result<List<Guid>>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, PaymentMethodEnum paymentMethod)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                return new Result<List<Guid>>()
                {
                    Error = 1,
                    Message = "Giỏ hàng không tồn tại hoặc không có sản phẩm nào",
                    Data = null
                };

            var selectedItems = cart.Items.Where(i => selectedCartItemIds.Contains(i.Id)).ToList();
            if (!selectedItems.Any())
                return new Result<List<Guid>>
                {
                    Error = 1,
                    Message = "Không có sản phẩm nào được chọn từ giỏ hàng",
                    Data = null
                };

            var orders = new List<Order>();

            if (paymentMethod == PaymentMethodEnum.Online)
            {
                //Tạo 1 Order duy nhất
                var order = new Order
                {
                    UserId = userId,
                    Status = OrderStatusEnum.WaitingForPayment,
                    PaymentMethod = paymentMethod,
                    CreationDate = DateTime.UtcNow,
                    OrderItems = selectedItems.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ArtisanId = i.Product.Artisan_id,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };
                order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                orders.Add(order);
                await _unitOfWork.orderRepository.AddAsync(order);

                foreach (var item in selectedItems)
                {
                    item.IsDeleted = true;
                }
            }
            else
            {
                // Với thanh toán tiền mặt, chia theo Artisan
                var grouped = selectedItems.GroupBy(i => i.Product.Artisan_id);
                foreach (var group in grouped)
                {
                    var order = new Order
                    {
                        UserId = userId,
                        Status = OrderStatusEnum.Pending,
                        PaymentMethod = paymentMethod,
                        CreationDate = DateTime.UtcNow,
                        OrderItems = group.Select(i => new OrderItem
                        {
                            ProductId = i.ProductId,
                            ArtisanId = i.Product.Artisan_id,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice
                        }).ToList()
                    };
                    order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                    orders.Add(order);
                    await _unitOfWork.orderRepository.AddAsync(order);

                    foreach (var item in group)
                    {
                        item.IsDeleted = true;
                    }
                }
            }

            await _unitOfWork.SaveChangeAsync();

            var orderIds = orders.Select(o => o.Id).ToList();
            return new Result<List<Guid>>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = orderIds
            };
        }


        public async Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, CreateDirectOrderDto dto)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(dto.ProductId);
            if (product == null || product.Quantity < dto.Quantity)
                return new Result<Guid>()
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại hoặc số lượng không đủ",
                    Data = Guid.Empty
                };

            var order = new Order
            {
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                Status = dto.PaymentMethod == PaymentMethodEnum.Online ? OrderStatusEnum.WaitingForPayment : OrderStatusEnum.Pending,
                PaymentMethod = dto.PaymentMethod,
                OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = dto.ProductId,
                    ArtisanId = product.Artisan_id,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price
                }
            },
                TotalPrice = product.Price * dto.Quantity
            };

            await _unitOfWork.orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();

            return new Result<Guid>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = order.Id
            };
        }

        public async Task<Result<string>> CreateVnPayUrlAsync(Guid orderId, HttpContext httpContext)
        {
            var order = await _unitOfWork.orderRepository.GetByIdAsync(orderId);
            if (order == null || order.PaymentMethod != PaymentMethodEnum.Online)
                return new Result<string>
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại hoặc không phải là đơn hàng trực tuyến",
                    Data = null
                };

            var url = await _payoutService.CreatePaymentUrl(order, httpContext);
            return new Result<string>()
            {
                Error = 0,
                Message = "Tạo URL thanh toán thành công",
                Data = url
            };
        }

        public async Task<Result<string>> HandleVnPayReturnAsync(IQueryCollection query)
        {
            var orderId = Guid.Parse(query["vnp_TxnRef"]);
            var order = await _unitOfWork.orderRepository.GetByIdAsync(orderId);
            if (order == null) return new Result<string>()
            {
                Error = 1,
                Message = "Đơn hàng không tồn tại",
                Data = null
            };

            var isValid = await _payoutService.ValidateReturnData(query);
            var responseCode = query["vnp_ResponseCode"].ToString();

            // Ghi log thanh toán
            var log = new Payment
            {
                OrderId = order.Id,
                TransactionNo = query["vnp_TransactionNo"],
                BankCode = query["vnp_BankCode"],
                ResponseCode = responseCode,
                SecureHash = query["vnp_SecureHash"],
                RawData = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))
            };
            await _unitOfWork.paymentRepository.AddAsync(log);

            if (isValid && responseCode == "00")
            {
                order.IsPaid = true;
                order.Status = OrderStatusEnum.Paid;
            }
            else
            {
                order.Status = OrderStatusEnum.Cancelled;
            }

            await _unitOfWork.SaveChangeAsync();
            return new Result<string>
            {
                Error = 0,
                Message = isValid ? "Thanh toán thành công" : "Thanh toán không hợp lệ",
                Data = null
            };
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto statusDto)
        {
            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại",
                    Data = false
                };
            }

            // Optional: Add validation for valid status transitions
            order.Status = statusDto.Status;
            order.ModificationDate = DateTime.UtcNow.AddHours(7);

            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>()
            {
                Error = 0,
                Message = "Cập nhật trạng thái đơn hàng thành công",
                Data = true
            };
        }
    }
}
