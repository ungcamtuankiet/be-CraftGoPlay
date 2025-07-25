using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.Order;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public async Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersAsync(int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var orders = await _unitOfWork.orderRepository.GetListOrderAsync(pageIndex, pageSize,status);
            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders);

            return new ResponseOrder<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng thành công",
                TotalPrice = (double?)orderDtos.Sum(o => o.TotalPrice),
                Count = orderDtos.Count,
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

        public async Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersByUserIdAsync(Guid userId, int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            int count = 0;

            if (userId != currentUserId)
            {
                return new ResponseOrder<List<ViewOrderDTO>>()
                {
                    Error = 2,
                    Message = "Bạn không có quyền xem đơn hàng của người dùng này",
                    Count = 0,
                    TotalPrice = 0,
                    Data = null
                };
            }

            var orders = await _unitOfWork.orderRepository.GetOrdersByUserIdAsync(userId,pageIndex, pageSize, status);
            
            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders);

            return new ResponseOrder<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng theo người dùng thành công",
                Count = orderDtos.Count,
                TotalPrice = (double?)orderDtos.Sum(o => o.TotalPrice),
                Data = orderDtos
            };
        }

        public async Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var currentArtisanId = _claimsService.GetCurrentUserId;

            if (artisanId != currentArtisanId)
            {
                return new ResponseOrder<List<ViewOrderDTO>>()
                {
                    Error = 2,
                    Message = "Bạn không có quyền xem đơn hàng của người dùng này",
                    Count = 0,
                    TotalPrice = 0,
                    Data = null
                };
            }

            var orders = await _unitOfWork.orderRepository.GetOrdersByArtisanIdAsync(artisanId, pageIndex, pageSize, status);
            var filteredOrders = orders.Select(order =>
            {
                // Lọc OrderItems chỉ giữ lại các item thuộc artisanId
                order.OrderItems = order.OrderItems.Where(oi => oi.ArtisanId == artisanId).ToList();
                return order;
            }).Where(order => order.OrderItems.Any()).ToList();

            var orderDtos = _mapper.Map<List<ViewOrderDTO>>(orders); // Use AutoMapper

            return new ResponseOrder<List<ViewOrderDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đơn hàng theo artisan thành công",
                Count = orderDtos.Count,
                TotalPrice = (double?)orderDtos.Sum(o => o.TotalPrice),
                Data = orderDtos
            };
        }

        public async Task<Result<List<Guid>>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, Guid address, PaymentMethodEnum paymentMethod)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            var transactionId = Guid.NewGuid();
            var getUserAddress = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId);
            var getAddressByUserId = await _unitOfWork.userAddressRepository.GetByIdAsync(address);
            var getAddressDefault = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            bool isValidAddress = await _unitOfWork.userAddressRepository.CheckAddressUser(address, userId);
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
                if(address == null)
                {
                    address = getAddressDefault.Id;
                }
                if(!isValidAddress)
                {
                    return new Result<List<Guid>>()
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ.",
                        Data = null
                    };
                }
                var order = new Order
                {
                    UserId = userId,
                    TransactionId = transactionId,
                    UserAddressId = address,
                    Status = OrderStatusEnum.AwaitingPayment,
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
                if (address == null)
                {
                    address = getAddressDefault.Id;
                }
                if (!isValidAddress)
                {
                    return new Result<List<Guid>>()
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ.",
                        Data = null
                    };
                }
                var grouped = selectedItems.GroupBy(i => i.Product.Artisan_id);
                foreach (var group in grouped)
                {
                    var order = new Order
                    {
                        UserId = userId,
                        Status = OrderStatusEnum.Created,
                        TransactionId = transactionId,
                        UserAddressId = address,
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

                        var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity < item.Quantity)
                            {
                                return new Result<List<Guid>>()
                                {
                                    Error = 1,
                                    Message = $"Sản phẩm {product.Name} không đủ hàng.",
                                    Data = null
                                };
                            }
                            product.Quantity = product.Quantity - item.Quantity;
                            _unitOfWork.productRepository.Update(product);
                        }
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


        public async Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, Guid address, CreateDirectOrderDto dto)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(dto.ProductId);
            var transactionId = Guid.NewGuid();
            var getUserAddress = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId);
            var getAddressByUserId = await _unitOfWork.userAddressRepository.GetByIdAsync(address);
            var getProduct = await _unitOfWork.productRepository.GetProductByProductId(dto.ProductId);
            var getAddressDefault = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            bool isValidAddress = await _unitOfWork.userAddressRepository.CheckAddressUser(address, userId);
            if (product == null || product.Quantity < dto.Quantity)
                return new Result<Guid>()
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại hoặc số lượng không đủ",
                    Data = Guid.Empty
                };
            var order = new Order();
            if (dto.PaymentMethod == PaymentMethodEnum.Online)
            {
                if (address == null)
                {
                    address = getAddressDefault.Id;
                }
                if (!isValidAddress)
                {
                    return new Result<Guid>()
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ.",
                    };
                }
                order.UserId = userId;
                order.Status = OrderStatusEnum.AwaitingPayment;
                order.TransactionId = transactionId;
                order.UserAddressId = address;
                order.PaymentMethod = dto.PaymentMethod;
                order.CreationDate = DateTime.UtcNow;
                order.Product_Amount = (double)(getProduct.Price * dto.Quantity);
                order.Delivery_Amount = 25000;
                order.OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = dto.ProductId,
                        ArtisanId = product.Artisan_id,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price
                    }
                };
                order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount);
                await _unitOfWork.orderRepository.AddAsync(order);
            }
            else
            {
                if (address == null)
                {
                    address = getAddressDefault.Id;
                }
                if (!isValidAddress)
                {
                    return new Result<Guid>()
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ."
                    };
                }
                order.UserId = userId;
                order.Status = OrderStatusEnum.Created;
                order.TransactionId = transactionId;
                order.UserAddressId = address;
                order.PaymentMethod = dto.PaymentMethod;
                order.CreationDate = DateTime.UtcNow;
                order.Product_Amount = (double)(getProduct.Price * dto.Quantity);
                order.Delivery_Amount = 25000;
                order.OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = dto.ProductId,
                        ArtisanId = product.Artisan_id,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price
                    }
                };
                order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount); 
                getProduct.Quantity = getProduct.Quantity - dto.Quantity;
                await _unitOfWork.productRepository.UpdateProduct(getProduct);
                await _unitOfWork.orderRepository.AddAsync(order);
            }

            await _unitOfWork.SaveChangeAsync();
            return new Result<Guid>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = order.Id
            };


            /*            var order = new Order
                        {
                            UserId = userId,
                            CreationDate = DateTime.UtcNow,
                            Status = dto.PaymentMethod == PaymentMethodEnum.Online ? OrderStatusEnum.AwaitingPayment : OrderStatusEnum.Created,
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
                        };*/
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

        public async Task<Result<object>> HandleVnPayReturnAsync(IQueryCollection query)
        {
            var orderId = Guid.Parse(query["vnp_TxnRef"]);
            var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(orderId);
            var order = await _unitOfWork.orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại",
                    Data = null
                };
            }

            var isValid = await _payoutService.ValidateReturnData(query);
            var responseCode = query["vnp_ResponseCode"].ToString();

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
                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        if (product.Quantity < item.Quantity)
                        {
                            return new Result<object>()
                            {
                                Error = 1,
                                Message = $"Sản phẩm {product.Name} không đủ tồn kho.",
                                Data = null
                            };
                        }

                        product.Quantity -= item.Quantity;
                        _unitOfWork.productRepository.Update(product);
                    }
                }

                order.IsPaid = true;
                order.Status = OrderStatusEnum.Created;
            }
            else
            {
                order.Status = OrderStatusEnum.Cancelled;
            }

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = isValid ? "Thanh toán thành công" : "Thanh toán không hợp lệ",
                Data = orderId
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

            order.Status = statusDto.Status;
            order.ModificationDate = DateTime.UtcNow.AddHours(7);

            // Set IsPaid to true when status is Completed
            if (statusDto.Status == OrderStatusEnum.Completed)
            {
                order.IsPaid = true;
            }

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
