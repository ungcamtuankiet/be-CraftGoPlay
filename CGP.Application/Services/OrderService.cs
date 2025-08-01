﻿using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Utils;
using CGP.Contract.Abstractions.Shared;
using CGP.Contract.Abstractions.VnPayService;
using CGP.Contract.DTO.GHN;
using CGP.Contract.DTO.Order;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using VNPAY.NET.Enums;
using static System.Net.Mime.MediaTypeNames;
using Transaction = CGP.Domain.Entities.Transaction;

namespace CGP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayoutService _payoutService;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IConfiguration _configuration;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "return-request";

        public OrderService(IUnitOfWork unitOfWork, IPayoutService payoutService, IMapper mapper, IClaimsService claimsService, IConfiguration configuration, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _payoutService = payoutService;
            _mapper = mapper;
            _claimsService = claimsService;
            _configuration = configuration;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersAsync(int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var orders = await _unitOfWork.orderRepository.GetListOrderAsync(pageIndex, pageSize, status);
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

            var orders = await _unitOfWork.orderRepository.GetOrdersByUserIdAsync(userId, pageIndex, pageSize, status);

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

        public async Task<Result<Guid>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, Dictionary<Guid, double> deliveryAmounts, Guid address, PaymentMethodEnum paymentMethod)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            var transactionId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var getUserAddress = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId);
            var getAddressByUserId = await _unitOfWork.userAddressRepository.GetByIdAsync(address);
            var getAddressDefault = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            bool isValidAddress = await _unitOfWork.userAddressRepository.CheckAddressUser(address, userId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

            if (cart == null || !cart.Items.Any())
                return new Result<Guid>
                {
                    Error = 1,
                    Message = "Giỏ hàng không tồn tại hoặc không có sản phẩm nào"
                };

            var selectedItems = cart.Items.Where(i => selectedCartItemIds.Contains(i.Id)).ToList();
            if (!selectedItems.Any())
                return new Result<Guid>
                {
                    Error = 1,
                    Message = "Không có sản phẩm nào được chọn từ giỏ hàng"
                };

            // Kiểm tra xem phí giao hàng có được cung cấp cho tất cả Artisan_id không
            var artisanIds = selectedItems.Select(i => i.Product.Artisan_id).Distinct().ToList();
            if (artisanIds.Any(id => !deliveryAmounts.ContainsKey(id)))
                return new Result<Guid>
                {
                    Error = 1,
                    Message = "Phí giao hàng không được cung cấp cho tất cả các shop."
                };

            var orders = new List<Order>();

            if (paymentMethod == PaymentMethodEnum.Online)
            {
                if (address == null)
                {
                    address = getAddressDefault.Id;
                }
                if (!isValidAddress)
                {
                    return new Result<Guid>
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ."
                    };
                }
                var grouped = selectedItems.GroupBy(i => i.Product.Artisan_id);
                foreach (var group in grouped)
                {
                    var artisanId = group.Key;
                    var deliveryAmount = deliveryAmounts[artisanId];

                    var order = new Order
                    {
                        UserId = userId,
                        TransactionId = transactionId,
                        UserAddressId = address,
                        Status = OrderStatusEnum.AwaitingPayment,
                        PaymentMethod = paymentMethod,
                        CreationDate = DateTime.UtcNow,
                        Delivery_Amount = deliveryAmount,
                        OrderItems = group.Select(i => new OrderItem 
                        {
                            ProductId = i.ProductId,
                            ArtisanId = i.Product.Artisan_id,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice
                        }).ToList()
                    };
                    order.Product_Amount = (double)order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                    order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount);
                    orders.Add(order);
                    await _unitOfWork.orderRepository.AddAsync(order);
                    foreach (var item in group) // Chỉ xử lý các mục trong group
                    {
                        item.IsDeleted = true;
                        var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity < item.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = $"Sản phẩm {product.Name} không đủ hàng."
                                };
                            }
                            product.Quantity = product.Quantity - item.Quantity;
                            _unitOfWork.productRepository.Update(product);
                        }
                    }
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
                    return new Result<Guid>
                    {
                        Error = 1,
                        Message = "Địa chỉ không hợp lệ."
                    };
                }
                var grouped = selectedItems.GroupBy(i => i.Product.Artisan_id);
                foreach (var group in grouped)
                {
                    var artisanId = group.Key;
                    var deliveryAmount = deliveryAmounts[artisanId];
                    var order = new Order
                    {
                        UserId = userId,
                        Status = OrderStatusEnum.Created,
                        TransactionId = transactionId,
                        UserAddressId = address,
                        PaymentMethod = paymentMethod,
                        CreationDate = DateTime.UtcNow,
                        Delivery_Amount = deliveryAmount,
                        OrderItems = group.Select(i => new OrderItem
                        {
                            ProductId = i.ProductId,
                            ArtisanId = i.Product.Artisan_id,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice
                        }).ToList()
                    };
                    await _unitOfWork.orderRepository.AddAsync(order);

                    foreach (var item in group)
                    {
                        item.IsDeleted = true;

                        var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity < item.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = $"Sản phẩm {product.Name} không đủ hàng.",
                                };
                            }
                            //order.Product_Amount = (double)(product.Price * item.Quantity);
                            product.Quantity = product.Quantity - item.Quantity;
                            _unitOfWork.productRepository.Update(product);
                        }
                    }
                    order.Product_Amount = (double)order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                    order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount);
                    //order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                    orders.Add(order);
                    var log = new Payment
                    {
                        OrderId = order.Id,
                        PaymentMethod = PaymentMethodEnum.Cash,
                    };
                    await _unitOfWork.paymentRepository.AddAsync(log);
                    var transaction = new Domain.Entities.Transaction
                    {
                        Id = Guid.NewGuid(),
                        UserId = order.UserId,
                        OrderId = order.Id,
                        Amount = order.TotalPrice,
                        Currency = "VND",
                        PaymentId = log.Id,
                        PaymentMethod = order.PaymentMethod,
                        TransactionStatus = (TransactionStatusEnum)order.Status,
                        TransactionDate = DateTime.UtcNow.AddHours(7),
                        DiscountAmount = 0,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),
                        Notes = $"Đặt hàng từ giỏ hàng với mã đơn hàng là {order.Id} bằng thanh toán tiền mặt.",
                        CreatedBy = order.UserId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow,
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);
                }
            }
            cart.IsCheckedOut = true;
            _unitOfWork.cartRepository.Update(cart);
            await _unitOfWork.SaveChangeAsync();

            var orderIds = orders.Select(o => o.Id).ToList();
            return new Result<Guid>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = transactionId
            };
        }


        public async Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, Guid address, double Delivery_Amount, CreateDirectOrderDto dto)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(dto.ProductId);
            var transactionId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var getUserAddress = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId);
            var getAddressByUserId = await _unitOfWork.userAddressRepository.GetByIdAsync(address);
            var getProduct = await _unitOfWork.productRepository.GetProductByProductId(dto.ProductId);
            var getAddressDefault = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            bool isValidAddress = await _unitOfWork.userAddressRepository.CheckAddressUser(address, userId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

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
                order.Delivery_Amount = Delivery_Amount;
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
                order.Delivery_Amount = Delivery_Amount;
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
                var log = new Payment
                {
                    Id = paymentId,
                    OrderId = order.Id,
                    PaymentMethod = PaymentMethodEnum.Cash,
                };
                var transaction = new Domain.Entities.Transaction
                {
                    Id = transactionId,
                    UserId = order.UserId,
                    OrderId = order.Id,
                    Amount = order.TotalPrice,
                    Currency = "VND",
                    PaymentId = paymentId,
                    PaymentMethod = order.PaymentMethod,
                    TransactionStatus = (TransactionStatusEnum)order.Status,
                    TransactionDate = DateTime.UtcNow.AddHours(7),
                    DiscountAmount = 0,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    Notes = $"Đặt hàng trực tiếp với mã đơn hàng là {order.Id} bằng thanh toán tiền mặt.",
                    CreatedBy = order.UserId,
                    IsDeleted = false,
                    CreationDate = DateTime.UtcNow.AddHours(7),
                };
                await _unitOfWork.transactionRepository.AddAsync(transaction);
                await _unitOfWork.paymentRepository.AddAsync(log);
            }

            await _unitOfWork.SaveChangeAsync();
            return new Result<Guid>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = transactionId
            };
        }

        public async Task<Result<string>> CreateVnPayUrlAsync(Guid transactionId, HttpContext httpContext)
        {
            var orders = await _unitOfWork.orderRepository.GetOrdersByTransactionIdAsync(transactionId);
            if (orders == null || !orders.Any())
            {
                return new Result<string>
                {
                    Error = 1,
                    Message = "Không tìm thấy đơn hàng với TransactionId tương ứng",
                    Data = null
                };
            }

            var totalAmount = orders.Sum(o => o.TotalPrice);

            var url = await _payoutService.CreatePaymentUrl(transactionId, totalAmount, httpContext);
            return new Result<string>()
            {
                Error = 0,
                Message = "Tạo URL thanh toán thành công",
                Data = url
            };
        }

        public async Task<Result<object>> HandleVnPayReturnAsync(IQueryCollection query)
        {
            // Lấy TransactionId thay vì OrderId
            var transactionId = Guid.Parse(query["vnp_TxnRef"]);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

            // Lấy tất cả đơn hàng theo TransactionId
            var orders = await _unitOfWork.orderRepository.GetOrdersByTransactionIdAsync(transactionId);
            if (orders == null || !orders.Any())
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy đơn hàng nào cho TransactionId.",
                    Data = null
                };
            }

            var isValid = await _payoutService.ValidateReturnData(query);
            var responseCode = query["vnp_ResponseCode"].ToString();
            var userId = orders.First().UserId;

            // Ghi log Payment (1 lần cho cả transaction)
            var payment = new Payment
            {
                OrderId = orders.First().Id, // lấy đại diện
                TransactionNo = query["vnp_TransactionNo"],
                BankCode = query["vnp_BankCode"],
                ResponseCode = responseCode,
                SecureHash = query["vnp_SecureHash"],
                CreatedBy = userId,
                PaymentMethod = PaymentMethodEnum.Online,
                RawData = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))
            };
            await _unitOfWork.paymentRepository.AddAsync(payment);

            decimal totalAmount = 0;

            if (isValid && responseCode == "00")
            {
                foreach (var order in orders)
                {
                    var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);

                    foreach (var item in orderItems)
                    {
                        var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity < item.Quantity)
                            {
                                return new Result<object>
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
                    totalAmount += order.TotalPrice;
                    var transaction = new Domain.Entities.Transaction
                    {
                        Id = Guid.NewGuid(),
                        Amount = totalAmount,
                        UserId = userId,
                        OrderId = order.Id,
                        PaymentId = payment.Id,
                        Currency = "VND",
                        PaymentMethod = PaymentMethodEnum.Online,
                        TransactionStatus = TransactionStatusEnum.Success,
                        TransactionDate = DateTime.UtcNow.AddHours(7),
                        DiscountAmount = 0,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),
                        Notes = $"Thanh toán đơn hàng từ giỏ hàng, tổng cộng {orders.Count} đơn.",
                        CreatedBy = userId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);

                    getWalletSystem.Balance = getWalletSystem.Balance + (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    var addMoneyToWalletSystem = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Purchase,
                        Description = $"Thanh toán đơn hàng {order.Id} có mức giá: {order.TotalPrice} với phương thức thanh toán VNPay.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);
                }
            }
            else
            {
                foreach (var order in orders)
                {
                    order.Status = OrderStatusEnum.Cancelled;
                }
            }
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = isValid ? "Thanh toán thành công" : "Thanh toán thất bại hoặc không hợp lệ.",
                Data = transactionId
            };
        }



        public async Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum statusDto)
        {
            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(orderId);
            var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(orderId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();
            var getPayment = await _unitOfWork.paymentRepository.GetPaymentByOrderId(order.Id);
            var getWalletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(order.UserId);
            var getTransaction = await _unitOfWork.transactionRepository.GetTransactionByOrderId(order.Id);

            if (order == null)
            {
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại",
                    Data = false
                };
            }

            if(statusDto == OrderStatusEnum.Cancelled)
            {
                if (statusDto == OrderStatusEnum.Rejected)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Đơn đang đã bị nghệ nhân từ chối.",
                        Data = false
                    };
                }
                
                if (order.PaymentMethod == PaymentMethodEnum.Online && order.IsPaid == true)
                {
                    if (order.Status == OrderStatusEnum.Shipped || order.Status == OrderStatusEnum.Delivered)
                    {
                        return new Result<bool>()
                        {
                            Error = 1,
                            Message = "Đơn hàng đang trong quá trình giao hàng nên không thể hủy",
                            Data = false
                        };
                    }

                    var transaction = new Transaction
                    {
                        UserId = order.UserId,
                        OrderId = order.Id,
                        Amount = order.TotalPrice,
                        PaymentId = getPayment.Id,
                        Currency = "VND",
                        PaymentMethod = order.PaymentMethod,
                        TransactionStatus = (TransactionStatusEnum)order.Status,
                        TransactionDate = DateTime.UtcNow.AddHours(7),
                        DiscountAmount = 0,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),
                        Notes = $"Hủy đơn hàng với mã đơn hàng là: {order.Id} với số tiền: {order.TotalPrice}.",
                        CreatedBy = order.UserId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);

                    getWalletSystem.Balance = getWalletSystem.Balance - (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    getWalletUser.Balance = getWalletUser.Balance + (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletUser);

                    var newWalletSystemTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletUser.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền cho đơn hàng {order.Id} với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletSystemTransaction);

                    var newWalletUserTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền cho đơn hàng {order.Id} với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);

                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);
                    order.Status = OrderStatusEnum.Cancelled;
                    _unitOfWork.orderRepository.Update(order);
                    order.ModificationDate = DateTime.UtcNow.AddHours(7);
                    await _unitOfWork.SaveChangeAsync();

                    return new Result<bool>()
                    {
                        Error = 0,
                        Message = "Hủy đơn hàng thành công",
                        Data = true
                    };
                }
            }

            if(statusDto == OrderStatusEnum.Rejected)
            {
                if (order.Status == OrderStatusEnum.Cancelled)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Đơn hàng không thể bị người dùng hủy.",
                        Data = false
                    };
                }

                if(order.Status == OrderStatusEnum.Rejected)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Đơn hàng đã được từ chối.",
                        Data = false
                    };
                }

                if(order.Status != OrderStatusEnum.Created)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Đơn hàng đang được vận chuyển hoặc đã giao thành công nên không thể từ chối.",
                        Data = false
                    };
                }

                if (order.PaymentMethod == PaymentMethodEnum.Online && order.IsPaid == true)
                {
                    var payent = new Payment()
                    {
                        OrderId = order.Id,
                        TransactionNo = getPayment.TransactionNo,
                        BankCode = getPayment.BankCode,
                        ResponseCode = "00",
                        SecureHash = getPayment.SecureHash,
                        PaymentMethod = PaymentMethodEnum.Online,
                        RawData = getPayment.RawData,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false,
                        IsRefunded = true
                    };
                    await _unitOfWork.paymentRepository.AddAsync(payent);

                    var transaction = new Transaction
                    {
                        Id = order.TransactionId,
                        UserId = order.UserId,
                        OrderId = order.Id,
                        Amount = order.TotalPrice,
                        PaymentId = payent.Id,
                        Currency = "VND",
                        PaymentMethod = order.PaymentMethod,
                        TransactionStatus = (TransactionStatusEnum)order.Status,
                        TransactionDate = DateTime.UtcNow.AddHours(7),
                        DiscountAmount = 0,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),
                        Notes = $"Từ chối đơn hàng với mã đơn hàng là: {order.Id} với số tiền: {order.TotalPrice}.",
                        CreatedBy = order.UserId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                    };

                    getWalletSystem.Balance = getWalletSystem.Balance - (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    getWalletUser.Balance = getWalletUser.Balance + (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletUser);

                    var newWalletSystemTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletUser.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền cho đơn hàng {order.Id} với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletSystemTransaction);

                    var newWalletUserTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền cho đơn hàng {order.Id} với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);

                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);
                    order.Status = OrderStatusEnum.Cancelled;
                    _unitOfWork.orderRepository.Update(order);
                    order.ModificationDate = DateTime.UtcNow.AddHours(7);
                    await _unitOfWork.SaveChangeAsync();
                    return new Result<bool>()
                    {
                        Error = 0,
                        Message = "Từ chối đơn hàng thành công",
                        Data = true
                    };
                }
            }

            order.Status = statusDto;
            order.ModificationDate = DateTime.UtcNow.AddHours(7);
            if (statusDto == OrderStatusEnum.Completed)
            {
                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                    product.QuantitySold = product.QuantitySold + item.Quantity;
                    _unitOfWork.productRepository.Update(product);
                }

                if (order.PaymentMethod == PaymentMethodEnum.Cash && order.IsPaid == false)
                {
                    getWalletSystem.Balance = getWalletSystem.Balance + (float)order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    var addMoneyToWalletSystem = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Purchase,
                        Description = $"Thanh toán đơn hàng {order.Id} có mức giá: {order.TotalPrice} với phương thức thanh toán COD.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);
                }
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
