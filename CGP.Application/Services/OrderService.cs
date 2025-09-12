using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.DashBoard;
using CGP.Contract.DTO.Order;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Transaction = CGP.Domain.Entities.Transaction;

namespace CGP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayoutService _payoutService;
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IConfiguration _configuration;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "return-request";

        public OrderService(IUnitOfWork unitOfWork, IPayoutService payoutService, IMapper mapper, IClaimsService claimsService, IConfiguration configuration, ICloudinaryService cloudinaryService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _payoutService = payoutService;
            _mapper = mapper;
            _claimsService = claimsService;
            _configuration = configuration;
            _cloudinaryService = cloudinaryService;
            _walletService = walletService;
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
/*            var currentArtisanId = _claimsService.GetCurrentUserId;

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
            }*/

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

        public async Task<Result<Guid>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, Dictionary<Guid, double> deliveryAmounts, Guid address, string voucherDeliveryCode, string? voucherProductCode, decimal Point,PaymentMethodEnum paymentMethod)
        {
            var cart = await _unitOfWork.cartRepository.GetCartByUserIdAsync(userId);
            var transactionId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var getUserAddress = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId);
            var getAddressByUserId = await _unitOfWork.userAddressRepository.GetByIdAsync(address);
            var getAddressDefault = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            bool isValidAddress = await _unitOfWork.userAddressRepository.CheckAddressUser(address, userId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();
            var getVoucherDelivery = await _unitOfWork.voucherRepository.CheckVoucherDelivery(voucherDeliveryCode);
            var getVoucherProduct = await _unitOfWork.voucherRepository.CheckVoucherProduct(voucherProductCode);
            var getUserPoint = await _unitOfWork.pointRepository.GetPointsByUserId(userId);
            double totalShipping = deliveryAmounts.Values.Sum();
            double discountProduct = 0;
            double discountDelivery = 0;
            double totalDiscount = 0;
            double totalPrice = 0;
            Guid? voucherId = null;

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
                double totalProductAmount = (double)grouped.Sum(g => g.Sum(i => i.Quantity * i.UnitPrice));
                foreach (var group in grouped)
                {
                    var artisanId = group.Key;
                    var deliveryAmount = deliveryAmounts[artisanId];
                    var order = new Order();
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
                            product.Quantity = product.Quantity - item.Quantity;
                            _unitOfWork.productRepository.Update(product);
                        }
                    }

                    order.OrderItems = group.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ArtisanId = i.Product.Artisan_id,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        Status = OrderStatusEnum.Created
                    }).ToList();
                    order.Product_Amount = (double)order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

                    if (voucherDeliveryCode == null && voucherProductCode == null)
                    {
                        discountProduct = 0;
                        discountDelivery = 0;
                    }

                    else
                    {
                        if (voucherProductCode != null)
                        {
                            var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherProduct.Id);
                            if (getUserVoucher.IsUsed)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá đã được sử dụng.",
                                };
                            }
                            if (getVoucherProduct == null)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm không tồn tại.",
                                };
                            }

                            if (getVoucherProduct.StartDate > DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm chưa bắt đầu sử dụng.",
                                };
                            }

                            if (getVoucherProduct.EndDate < DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm đã hết hạn sử dụng.",
                                };
                            }

                            if (getVoucherProduct.UsedCount == getVoucherProduct.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm đã hết lượt sử dụng.",
                                };
                            }

                            if (getVoucherProduct.IsActive == false)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá không còn hoạt động.",
                                };
                            }

                            if (getVoucherProduct.PaymentMethod != paymentMethod)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm chỉ được sử dụng cho giao dịch tiền mặt.",
                                };
                            }

                            if (totalProductAmount < getVoucherProduct.MinOrderValue)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho sản phẩm.",
                                };
                            }

                            if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.Percentage)
                            {
                                discountProduct = Math.Floor(order.Product_Amount * ((getVoucherProduct.Discount / grouped.Count()) / 100.0));

                            }
                            else if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                            {
                                discountProduct = Math.Floor(getVoucherProduct.Discount / grouped.Count());
                                if (discountProduct > order.Product_Amount)
                                {
                                    discountProduct = order.Product_Amount;
                                }
                                else
                                {
                                    discountProduct = discountProduct;
                                }
                            }

                            getVoucherProduct.UsedCount++;
                            getUserVoucher.IsUsed = true;
                        }
                        else
                        {
                            discountProduct = 0;
                        }

                        if (voucherDeliveryCode != null)
                        {
                            var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherDelivery.Id);
                            if (getUserVoucher.IsUsed)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá đã được sử dụng.",
                                };
                            }
                            if (getVoucherDelivery == null)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển không tồn tại.",
                                };
                            }

                            if (getVoucherDelivery.StartDate > DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm cho vận chuyển giá chưa bắt đầu sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.EndDate < DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển đã hết hạn sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.UsedCount == getVoucherDelivery.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển đã hết lượt sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.IsActive == false)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá không còn hoạt động.",
                                };
                            }

                            if (getVoucherDelivery.PaymentMethod != paymentMethod)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển chỉ được sử dụng cho giao dịch online.",
                                };
                            }

                            if (totalProductAmount < getVoucherDelivery.MinOrderValue || totalProductAmount > getVoucherDelivery.MaxDiscountAmount)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho phí vận chuyển.",
                                };
                            }


                            if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.Percentage)
                            {
                                discountDelivery = Math.Floor(deliveryAmount * ((getVoucherDelivery.Discount / 100.0) / grouped.Count()));
                            }
                            else if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                            {
                                discountDelivery = Math.Floor(getVoucherDelivery.Discount / grouped.Count());
                                if (discountDelivery > deliveryAmount)
                                {
                                    discountDelivery = deliveryAmount;
                                }
                            }
                            getVoucherDelivery.UsedCount++;
                            getUserVoucher.IsUsed = true;
                        }
                        else
                        {
                            discountDelivery = 0;
                        }
                    }

                    order.UserId = userId;
                    order.TransactionId = transactionId;
                    order.Status = OrderStatusEnum.AwaitingPayment;
                    order.PaymentMethod = paymentMethod;
                    order.CreationDate = DateTime.UtcNow.AddHours(7);
                    order.Delivery_Amount = deliveryAmount;
                    order.ProductDiscount = discountProduct;
                    order.DeliveryDiscount = discountDelivery;
                    order.Point = Point;
                    order.PointDiscount = Point * 100;
                    order.TotalDiscount = order.ProductDiscount + order.DeliveryDiscount + (double)(order.PointDiscount);
                    order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount - order.TotalDiscount);
                    orders.Add(order);
                    await _unitOfWork.orderRepository.AddAsync(order);
                    

                    var orderAddress = new OrderAddress
                    {
                        OrderId = order.Id,
                        FullName = getAddressByUserId.FullName,
                        PhoneNumber = getAddressByUserId.PhoneNumber,
                        FullAddress = getAddressByUserId.FullAddress,
                        ProviceId = getAddressByUserId.ProviceId,
                        ProviceName = getAddressByUserId.ProviceName,
                        DistrictId = getAddressByUserId.DistrictId,
                        DistrictName = getAddressByUserId.DistrictName,
                        WardCode = getAddressByUserId.WardCode,
                        WardName = getAddressByUserId.WardName,
                        HomeNumber = getAddressByUserId.HomeNumber
                    };
                    await _unitOfWork.orderAddressRepository.AddAsync(orderAddress);

                    if (getVoucherDelivery != null)
                    {
                        await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                        {
                            OrderId = order.Id,
                            VoucherId = getVoucherDelivery.Id
                        });
                        _unitOfWork.voucherRepository.Update(getVoucherDelivery);
                    }
                    if (getVoucherProduct != null)
                    {
                        await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                        {
                            OrderId = order.Id,
                            VoucherId = getVoucherProduct.Id
                        });
                        _unitOfWork.voucherRepository.Update(getVoucherProduct);
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
                double totalProductAmount = (double)grouped.Sum(g => g.Sum(i => i.Quantity * i.UnitPrice));
                foreach (var group in grouped)
                {
                    var artisanId = group.Key;
                    var deliveryAmount = deliveryAmounts[artisanId];
                    var order = new Order();
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
                            product.Quantity = product.Quantity - item.Quantity;
                            _unitOfWork.productRepository.Update(product);
                        }
                    }

                    order.OrderItems = group.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ArtisanId = i.Product.Artisan_id,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        Status = OrderStatusEnum.Created
                    }).ToList();
                    order.Product_Amount = (double)order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                    if (voucherDeliveryCode == null && voucherProductCode == null)
                    {
                        discountProduct = 0;
                        discountDelivery = 0;
                    }

                    else
                    {
                        if (voucherProductCode != null)
                        {
                            var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherProduct.Id);
                            if (getUserVoucher.IsUsed)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá đã được sử dụng.",
                                };
                            }
                            if (getVoucherProduct == null)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm không tồn tại.",
                                };
                            }

                            if (getVoucherProduct.StartDate > DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm chưa bắt đầu sử dụng.",
                                };
                            }

                            if (getVoucherProduct.EndDate < DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm đã hết hạn sử dụng.",
                                };
                            }

                            if (getVoucherProduct.UsedCount == getVoucherProduct.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm đã hết lượt sử dụng.",
                                };
                            }

                            if (getVoucherProduct.IsActive == false)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá không còn hoạt động.",
                                };
                            }

                            if (getVoucherProduct.PaymentMethod != paymentMethod)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho sản phẩm chỉ được sử dụng cho giao dịch tiền mặt.",
                                };
                            }

                            if (totalProductAmount < getVoucherProduct.MinOrderValue)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho sản phẩm.",
                                };
                            }

                            if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.Percentage)
                            {
                                discountProduct = Math.Floor(order.Product_Amount * ((getVoucherProduct.Discount / grouped.Count()) / 100.0));
                            
                            }
                            else if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                            {
                                discountProduct = Math.Floor(getVoucherProduct.Discount / grouped.Count());
                                if (discountProduct > order.Product_Amount)
                                {
                                    discountProduct = order.Product_Amount;
                                }
                                else
                                {
                                    discountProduct = discountProduct;
                                }
                            }

                            getVoucherProduct.UsedCount++;
                            getUserVoucher.IsUsed = true;
                        }
                        else
                        {
                            discountProduct = 0;
                        }

                        if (voucherDeliveryCode != null)
                        {
                            var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherDelivery.Id);
                            if (getUserVoucher.IsUsed)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá đã được sử dụng.",
                                };
                            }
                            if (getVoucherDelivery == null)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển không tồn tại.",
                                };
                            }

                            if (getVoucherDelivery.StartDate > DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm cho vận chuyển giá chưa bắt đầu sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.EndDate < DateTime.UtcNow.AddHours(7))
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển đã hết hạn sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.UsedCount == getVoucherDelivery.Quantity)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển đã hết lượt sử dụng.",
                                };
                            }

                            if (getVoucherDelivery.IsActive == false)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá không còn hoạt động.",
                                };
                            }

                            if (getVoucherDelivery.PaymentMethod != paymentMethod)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Mã giảm giá cho vận chuyển chỉ được sử dụng cho giao dịch tiền mặt.",
                                };
                            }

                            if (totalProductAmount < getVoucherDelivery.MinOrderValue || totalProductAmount > getVoucherDelivery.MaxDiscountAmount)
                            {
                                return new Result<Guid>()
                                {
                                    Error = 1,
                                    Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho phí vận chuyển.",
                                };
                            }


                            if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.Percentage)
                            {
                                discountDelivery = Math.Floor(deliveryAmount * ((getVoucherDelivery.Discount / 100.0)/grouped.Count()));
                            }
                            else if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                            {
                                discountDelivery = Math.Floor(getVoucherDelivery.Discount / grouped.Count());
                                if (discountDelivery > deliveryAmount)
                                {
                                    discountDelivery = deliveryAmount;
                                }
                            }
                            getVoucherDelivery.UsedCount++;
                            getUserVoucher.IsUsed = true;
                        }
                        else
                        {
                            discountDelivery = 0;
                        }
                    }

                    order.UserId = userId;
                    order.TransactionId = transactionId;
                    order.Status = OrderStatusEnum.Created;
                    order.PaymentMethod = paymentMethod;
                    order.CreationDate = DateTime.UtcNow.AddHours(7);
                    order.Delivery_Amount = deliveryAmount;
                    order.ProductDiscount = discountProduct;
                    order.DeliveryDiscount = discountDelivery;
                    order.Point = Point;
                    order.PointDiscount = Point * 100;
                    order.TotalDiscount = order.ProductDiscount + order.DeliveryDiscount + (double)(order.PointDiscount);
                    order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount - order.TotalDiscount);
                    await _unitOfWork.orderRepository.AddAsync(order);
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
                        VoucherId = voucherId,
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
                        CreationDate = DateTime.UtcNow.AddHours(7),
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);

                    var orderAddress = new OrderAddress
                    {
                        OrderId = order.Id,
                        FullName = getAddressByUserId.FullName,
                        PhoneNumber = getAddressByUserId.PhoneNumber,
                        FullAddress = getAddressByUserId.FullAddress,
                        ProviceId = getAddressByUserId.ProviceId,
                        ProviceName = getAddressByUserId.ProviceName,
                        DistrictId = getAddressByUserId.DistrictId,
                        DistrictName = getAddressByUserId.DistrictName,
                        WardCode = getAddressByUserId.WardCode,
                        WardName = getAddressByUserId.WardName,
                        HomeNumber = getAddressByUserId.HomeNumber
                    };

                    await _unitOfWork.orderAddressRepository.AddAsync(orderAddress);

                    if (getVoucherDelivery != null)
                    {
                        await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                        {
                            OrderId = order.Id,
                            VoucherId = getVoucherDelivery.Id
                        });
                        _unitOfWork.voucherRepository.Update(getVoucherDelivery);
                    }
                    if (getVoucherProduct != null)
                    {
                        await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                        {
                            OrderId = order.Id,
                            VoucherId = getVoucherProduct.Id
                        });
                        _unitOfWork.voucherRepository.Update(getVoucherProduct);
                    }
                }
            }
            //cart.IsCheckedOut = true;
            if (!cart.Items.Any(i => !i.IsDeleted))
            {
                cart.IsCheckedOut = true;
            }
            else
            {
                cart.IsCheckedOut = false;
            }
            _unitOfWork.cartRepository.Update(cart);
            if (Point > 0)
            {
                getUserPoint.Amount = (int)(getUserPoint.Amount - Point);
                var pointTransaction = new PointTransaction
                {
                    Point_Id = getUserPoint.Id,
                    Amount = Point,
                    Description = $"Sử dụng {Point} điểm để đặt hàng.",
                    Status = PointTransactionEnum.Redeemed,
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                };
                await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
            }
            await _unitOfWork.SaveChangeAsync();
            return new Result<Guid>()
            {
                Error = 0,
                Message = "Đặt hàng thành công",
                Data = transactionId
            };
        }

        public async Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, Guid address, double Delivery_Amount, string voucherDeliveryCode, string? voucherProductCode, decimal Point,CreateDirectOrderDto dto)
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
            var getVoucherDelivery = await _unitOfWork.voucherRepository.CheckVoucherDelivery(voucherDeliveryCode);
            var getVoucherProduct = await _unitOfWork.voucherRepository.CheckVoucherProduct(voucherProductCode);
            var getUserPoint = await _unitOfWork.pointRepository.GetPointsByUserId(userId);
            double discountProduct = 0;
            double discountDelivery = 0;
            double totalDiscount = 0;
            Guid? voucherId = null;

            if (product == null || product.Quantity < dto.Quantity)
                return new Result<Guid>()
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại hoặc số lượng không đủ",
                    Data = Guid.Empty
                };
            var order = new Order();
            order.Product_Amount = (double)(dto.Quantity * product.Price);
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
                        Message = "Địa chỉ không hợp lệ."
                    };
                }


                if (voucherDeliveryCode == null && voucherProductCode == null)
                {
                    discountDelivery = 0;
                    discountProduct = 0;
                }

                else
                {
                    if (voucherProductCode != null)
                    {
                        var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherProduct.Id);
                        if (getUserVoucher.IsUsed)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá đã được sử dụng.",
                            };
                        }
                        if (getVoucherProduct == null)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm không tồn tại.",
                            };
                        }

                        if (getVoucherProduct.StartDate > DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm chưa bắt đầu sử dụng.",
                            };
                        }

                        if (getVoucherProduct.EndDate < DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm đã hết hạn sử dụng.",
                            };
                        }

                        if (getVoucherProduct.UsedCount == getVoucherProduct.Quantity)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm đã hết lượt sử dụng.",
                            };
                        }

                        if (getVoucherProduct.PaymentMethod != dto.PaymentMethod)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm chỉ được sử dụng cho giao dịch online.",
                            };
                        }

                        if ((double)(product.Price * dto.Quantity) < getVoucherProduct.MinOrderValue)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho sản phẩm.",
                            };
                        }

                        if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.Percentage)
                        {
                            discountProduct = Math.Floor(order.Product_Amount * (getVoucherProduct.Discount / 100.0));
                        }
                        else if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                        {
                            if (getVoucherProduct.Discount > order.Product_Amount)
                            {
                                discountProduct = order.Product_Amount;
                            }
                            else
                            {
                                discountProduct = getVoucherProduct.Discount;
                            }
                        }

                        getVoucherProduct.UsedCount++;
                        getUserVoucher.IsUsed = true;
                    }
                    else
                    {
                        order.ProductDiscount = 0;
                    }

                    if (voucherDeliveryCode != null)
                    {
                        var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherDelivery.Id);
                        if (getUserVoucher.IsUsed)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá đã được sử dụng.",
                            };
                        }
                        if (getVoucherDelivery == null)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển không tồn tại.",
                            };
                        }

                        if (order.Product_Amount < getVoucherDelivery.MinOrderValue)
                        {
                            return new Result<Guid>
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá vận chuyển."
                            };
                        }

                        if (getVoucherDelivery.StartDate > DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm cho vận chuyển giá chưa bắt đầu sử dụng.",
                            };
                        }

                        if (getVoucherDelivery.EndDate < DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển đã hết hạn sử dụng.",
                            };
                        }

                        if (getVoucherDelivery.UsedCount == getVoucherDelivery.Quantity)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển đã hết lượt sử dụng.",
                            };
                        }
/*
                        if (getVoucherDelivery.IsActive == false || getVoucherProduct.IsActive == false)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá không còn hoạt động.",
                            };
                        }*/

                        if (getVoucherDelivery.PaymentMethod != dto.PaymentMethod)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển chỉ được sử dụng cho giao dịch online.",
                            };
                        }

                        if ((double)(product.Price * dto.Quantity) < getVoucherDelivery.MinOrderValue || (double)(product.Price * dto.Quantity) > getVoucherDelivery.MaxDiscountAmount)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho phí vận chuyển.",
                            };
                        }


                        if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.Percentage)
                        {
                            discountDelivery = Math.Floor((Delivery_Amount * (getVoucherDelivery.Discount / 100.0)));
                        }
                        else if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                        {
                            if (getVoucherDelivery.Discount > Delivery_Amount)
                            {
                                discountDelivery = Delivery_Amount;
                            }
                            else
                            {
                                discountDelivery = getVoucherDelivery.Discount;
                            }
                        }

                        getVoucherDelivery.UsedCount++;
                        getUserVoucher.IsUsed = true;
                    }
                    else
                    {
                        order.DeliveryDiscount = 0;
                    }
                }

                order.UserId = userId;
                order.Status = OrderStatusEnum.AwaitingPayment;
                order.TransactionId = transactionId;
                order.ProductDiscount = discountProduct;
                order.DeliveryDiscount = discountDelivery;
                order.Point = Point;
                order.PointDiscount = Point * 100;
                order.PaymentMethod = dto.PaymentMethod;
                order.Delivery_Amount = Delivery_Amount;
                order.CreationDate = DateTime.UtcNow.AddHours(7);
                order.TotalDiscount = order.ProductDiscount + order.DeliveryDiscount + (double)(order.PointDiscount);
                order.OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = dto.ProductId,
                        ArtisanId = product.Artisan_id,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        Status = OrderStatusEnum.Created
                    }
                };
                order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount - order.TotalDiscount);
                await _unitOfWork.orderRepository.AddAsync(order);

                var orderAddress = new OrderAddress
                {
                    OrderId = order.Id,
                    FullName = getAddressByUserId.FullName,
                    PhoneNumber = getAddressByUserId.PhoneNumber,
                    FullAddress = getAddressByUserId.FullAddress,
                    ProviceId = getAddressByUserId.ProviceId,
                    ProviceName = getAddressByUserId.ProviceName,
                    DistrictId = getAddressByUserId.DistrictId,
                    DistrictName = getAddressByUserId.DistrictName,
                    WardCode = getAddressByUserId.WardCode,
                    WardName = getAddressByUserId.WardName,
                    HomeNumber = getAddressByUserId.HomeNumber
                };

                await _unitOfWork.orderAddressRepository.AddAsync(orderAddress);
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


                if (voucherDeliveryCode == null && voucherProductCode == null)
                {
                    order.DeliveryDiscount = 0;
                    order.ProductDiscount = 0;
                }

                else
                {
                    if (voucherProductCode != null)
                    {
                        var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherProduct.Id);
                        if (getUserVoucher.IsUsed)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá đã được sử dụng.",
                            };
                        }
                        if ( getVoucherProduct == null)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm không tồn tại.",
                            };
                        }

                        if (getVoucherProduct.StartDate > DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm chưa bắt đầu sử dụng.",
                            };
                        }

                        if (getVoucherProduct.EndDate < DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm đã hết hạn sử dụng.",
                            };
                        }

                        if (getVoucherProduct.UsedCount == getVoucherProduct.Quantity)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm đã hết lượt sử dụng.",
                            };
                        }

                        if (getVoucherProduct.PaymentMethod != dto.PaymentMethod)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho sản phẩm chỉ được sử dụng cho giao dịch tiền mặt.",
                            };
                        }

                        if ((double)(product.Price * dto.Quantity) < getVoucherProduct.MinOrderValue)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho sản phẩm.",
                            };
                        }

                        if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.Percentage)
                        {
                            discountProduct = Math.Floor(order.Product_Amount * (getVoucherProduct.Discount / 100.0));
                        }
                        else if (getVoucherProduct.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                        {
                            if (getVoucherProduct.Discount > order.Product_Amount)
                            {
                                discountProduct = order.Product_Amount;
                            }
                            else
                            {
                                discountProduct = getVoucherProduct.Discount;
                            }
                        }

                        getVoucherProduct.UsedCount++;
                        getUserVoucher.IsUsed = true;
                    }
                    else
                    {
                        order.ProductDiscount = 0;
                    }

                    if (voucherDeliveryCode != null)
                    {
                        var getUserVoucher = await _unitOfWork.userVoucherRepository.GetUserVoucher(userId, getVoucherDelivery.Id);
                        if (getUserVoucher.IsUsed)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá đã được sử dụng.",
                            };
                        }
                        if ( getVoucherDelivery == null)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển không tồn tại.",
                            };
                        }

                        if (order.Product_Amount < getVoucherDelivery.MinOrderValue)
                        {
                            return new Result<Guid>
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá vận chuyển."
                            };
                        }

                        if (getVoucherDelivery.StartDate > DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm cho vận chuyển giá chưa bắt đầu sử dụng.",
                            };
                        }

                        if (getVoucherDelivery.EndDate < DateTime.UtcNow.AddHours(7))
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển đã hết hạn sử dụng.",
                            };
                        }

                        if (getVoucherDelivery.UsedCount == getVoucherDelivery.Quantity)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển đã hết lượt sử dụng.",
                            };
                        }

/*                        if (getVoucherDelivery.IsActive == false || getVoucherProduct.IsActive == false)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá không còn hoạt động.",
                            };
                        }*/

                        if (getVoucherDelivery.PaymentMethod != dto.PaymentMethod)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Mã giảm giá cho vận chuyển chỉ được sử dụng cho giao dịch tiền mặt.",
                            };
                        }

                        if ((double)(product.Price * dto.Quantity) < getVoucherDelivery.MinOrderValue || (double)(product.Price * dto.Quantity) > getVoucherDelivery.MaxDiscountAmount)
                        {
                            return new Result<Guid>()
                            {
                                Error = 1,
                                Message = "Đơn hàng không đủ điều kiện áp dụng mã giảm giá cho phí vận chuyển.",
                            };
                        }


                        if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.Percentage)
                        {
                            discountDelivery = Math.Floor((Delivery_Amount * (getVoucherDelivery.Discount / 100.0)));
                        }
                        else if (getVoucherDelivery.DiscountType == VoucherDiscountTypeEnum.FixedAmount)
                        {
                            if(getVoucherDelivery.Discount > Delivery_Amount)
                            {
                                discountDelivery = Delivery_Amount;
                            }
                            else
                            {
                                discountDelivery = getVoucherDelivery.Discount;
                            }
                        }

                        getVoucherDelivery.UsedCount++;
                        getUserVoucher.IsUsed = true;
                    }
                    else
                    {
                        order.DeliveryDiscount = 0;
                    }
                }

                order.UserId = userId;
                order.Status = OrderStatusEnum.Created;
                order.TransactionId = transactionId;
                order.PaymentMethod = dto.PaymentMethod;
                order.CreationDate = DateTime.UtcNow.AddHours(7);
                order.Delivery_Amount = Delivery_Amount;
                order.DeliveryDiscount = discountDelivery;
                order.ProductDiscount = discountProduct;
                order.Point = Point;
                order.PointDiscount = Point * 100;
                order.TotalDiscount = order.ProductDiscount + order.DeliveryDiscount + (double)(order.PointDiscount);
                order.Product_Amount = (double)(dto.Quantity * product.Price);
                order.OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = dto.ProductId,
                        ArtisanId = product.Artisan_id,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        Status = OrderStatusEnum.Created
                    }
                }; 
                order.TotalPrice = (decimal)(order.Product_Amount + order.Delivery_Amount - order.TotalDiscount);
                getProduct.Quantity = getProduct.Quantity - dto.Quantity;
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
                _unitOfWork.productRepository.Update(getProduct);

                var orderAddress = new OrderAddress
                {
                    OrderId = order.Id,
                    FullName = getAddressByUserId.FullName,
                    PhoneNumber = getAddressByUserId.PhoneNumber,
                    FullAddress = getAddressByUserId.FullAddress,
                    ProviceId = getAddressByUserId.ProviceId,
                    ProviceName = getAddressByUserId.ProviceName,
                    DistrictId = getAddressByUserId.DistrictId,
                    DistrictName = getAddressByUserId.DistrictName,
                    WardCode = getAddressByUserId.WardCode,
                    WardName = getAddressByUserId.WardName,
                    HomeNumber = getAddressByUserId.HomeNumber
                };

                await _unitOfWork.orderAddressRepository.AddAsync(orderAddress);

                if(getVoucherDelivery != null)
                {
                    await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                    {
                        OrderId = order.Id,
                        VoucherId = getVoucherDelivery.Id
                    });
                    _unitOfWork.voucherRepository.Update(getVoucherDelivery);
                }
                if(getVoucherProduct != null)
                {
                    await _unitOfWork.orderVoucherRepository.AddAsync(new OrderVoucher
                    {
                        OrderId = order.Id,
                        VoucherId = getVoucherProduct.Id
                    });
                    _unitOfWork.voucherRepository.Update(getVoucherProduct);
                }
            }

            if(Point > 0)
            {
                getUserPoint.Amount = (int)(getUserPoint.Amount - Point);
                var pointTransaction = new PointTransaction
                {
                    Point_Id = getUserPoint.Id,
                    Amount = Point,
                    Description = $"Sử dụng {Point} điểm để đặt hàng.",
                    Status = PointTransactionEnum.Redeemed,
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                };
                await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);
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
                        item.Status = OrderStatusEnum.Created;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
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
                        Notes = @$"Thanh toán thành công đơn hàng ""{order.Id}"", tổng cộng {orders.Count} đơn.",
                        CreatedBy = userId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow.AddHours(7)
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);

                    getWalletSystem.PendingBalance = getWalletSystem.PendingBalance + order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    var addMoneyToWalletSystem = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Purchase,
                        Description = @$"Thanh toán đơn hàng ""{order.Id}"" có mức giá: {order.TotalPrice}VND với phương thức thanh toán VNPay.",
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
                    order.Status = OrderStatusEnum.PaymentFailed;
                    totalAmount += order.TotalPrice;
                    var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);
                    foreach (var item in orderItems)
                    {
                        item.Status = OrderStatusEnum.PaymentFailed;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
                    }

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
                        Notes = @$"Thanh toán thất bại đơn hàng ""{order.Id}"", tổng cộng {orders.Count} đơn.",
                        CreatedBy = userId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow.AddHours(7)
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);
                }
                await _unitOfWork.SaveChangeAsync();
                return new Result<object>
                {
                    Error = 1,
                    Message = "Thanh toán thất bại hoặc không hợp lệ.",
                    Data = null
                };
            }
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Thanh toán thành công",
                Data = transactionId
            };
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum statusDto, ReasonDeliveryFailed reason)
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

            if (statusDto == null)
            {
                return new Result<bool>()
                {
                    Error = 1,
                    Message = "Trạng thái đơn hàng không được để trống.",
                    Data = false
                };
            }
            var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(orderId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();
            var getPayment = await _unitOfWork.paymentRepository.GetPaymentByOrderId(order.Id);
            var getWalletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(order.UserId);
            var getTransaction = await _unitOfWork.transactionRepository.GetTransactionByOrderId(order.Id);

            if (statusDto == OrderStatusEnum.Cancelled)
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
                        Notes = $"Nhận tiền hủy đơn hàng với mã đơn hàng là: {order.Id} với số tiền: {order.TotalPrice}.",
                        CreatedBy = order.UserId,
                        IsDeleted = false,
                        CreationDate = DateTime.UtcNow.AddHours(7),
                    };
                    await _unitOfWork.transactionRepository.AddAsync(transaction);

                    getWalletSystem.PendingBalance = getWalletSystem.PendingBalance - order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    getWalletUser.AvailableBalance = getWalletUser.AvailableBalance + order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletUser);

                    var newWalletSystemTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletUser.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền tới người dùng cho đơn hàng:{order.Id} bị hủy với số tiền: {order.TotalPrice}.",
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
                        Description = $"Nhận tiền hoàn lại cho đơn hàng:{order.Id} đã hủy với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);

                    order.Status = OrderStatusEnum.Cancelled;

                    foreach (var item in orderItems)
                    {
                        item.Status = OrderStatusEnum.Cancelled;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
                    }

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

            if (statusDto == OrderStatusEnum.Rejected)
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

                if (order.Status == OrderStatusEnum.Rejected)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Đơn hàng đã được từ chối.",
                        Data = false
                    };
                }

                if (order.Status != OrderStatusEnum.Created)
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

                    getWalletSystem.PendingBalance = getWalletSystem.PendingBalance - order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletSystem);

                    getWalletUser.AvailableBalance = getWalletUser.AvailableBalance + order.TotalPrice;
                    _unitOfWork.walletRepository.Update(getWalletUser);

                    var newWalletSystemTransaction = new WalletTransaction
                    {
                        Wallet_Id = getWalletUser.Id,
                        Amount = order.TotalPrice,
                        Type = WalletTransactionTypeEnum.Refund,
                        Description = $"Hoàn tiền tới người dùng cho đơn hàng:{order.Id} bị từ chối  với số tiền: {order.TotalPrice}.",
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
                        Description = $"Nhận tiền hoàn lại cho đơn hàng:{order.Id} bị từ chối với số tiền: {order.TotalPrice}.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(newWalletUserTransaction);

                    order.Status = OrderStatusEnum.Rejected;
                    _unitOfWork.orderRepository.Update(order);

                    foreach (var item in orderItems)
                    {
                        item.Status = OrderStatusEnum.Rejected;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
                    }
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

            if (statusDto == OrderStatusEnum.Delivered)
            {

                if (order.PaymentMethod == PaymentMethodEnum.Cash && order.IsPaid == false)
                {
                    foreach (var item in orderItems)
                    {
                        getWalletSystem.PendingBalance = getWalletSystem.PendingBalance + order.TotalPrice;
                        _unitOfWork.walletRepository.Update(getWalletSystem);

                        var addMoneyToWalletSystem = new WalletTransaction
                        {
                            Wallet_Id = getWalletSystem.Id,
                            Amount = order.TotalPrice,
                            Type = WalletTransactionTypeEnum.Purchase,
                            Description = @$"Thanh toán đơn hàng ""{order.Id}"" có mức giá {order.TotalPrice}VNĐ với phương thức thanh toán COD.",
                            CreationDate = DateTime.UtcNow.AddHours(7),
                            CreatedAt = DateTime.UtcNow.AddHours(7),
                            IsDeleted = false
                        };
                        await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);
                    }
                    order.ModificationDate = DateTime.UtcNow.AddHours(7);
                }
                order.ModificationDate = DateTime.UtcNow.AddHours(7);
                order.IsPaid = true;
            }

            if (statusDto == OrderStatusEnum.DeliveryAttemptFailed)
            {
                if (order.Status != OrderStatusEnum.Shipped)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Chỉ có thể cập nhật trạng thái khi đơn hàng đang trong quá trình vận chuyển.",
                        Data = false
                    };
                }

                if (reason == null || reason == ReasonDeliveryFailed.Empty)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Cần có lý do cho việc giao hàng không thành công.",
                        Data = false
                    };
                }
                order.DeliveriesCount++;
                if (order.DeliveriesCount >= 3)
                {
                    order.Status = OrderStatusEnum.DeliveryFailed;
                    order.ReasonDeliveryFailed = reason;
                    order.ModificationDate = DateTime.UtcNow.AddHours(7);
                    foreach (var item in orderItems)
                    {
                        item.Status = OrderStatusEnum.DeliveryFailed;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
                    }
                    await _unitOfWork.SaveChangeAsync();
                    return new Result<bool>()
                    {
                        Error = 0,
                        Message = "Đơn hàng đã thử giao 3 lần không thành công và được đánh dấu là giao hàng thất bại.",
                        Data = true
                    };
                }
                else
                {
                    order.Status = statusDto;
                    order.ReasonDeliveryFailed = reason;
                    order.ModificationDate = DateTime.UtcNow.AddHours(7);
                    foreach (var item in orderItems)
                    {
                        item.Status = OrderStatusEnum.DeliveryAttemptFailed;
                        item.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderItemRepository.Update(item);
                    }
                }
            }

            if (statusDto == OrderStatusEnum.Completed)
            {
                if (order.Status != OrderStatusEnum.Delivered)
                {
                    return new Result<bool>()
                    {
                        Error = 1,
                        Message = "Chỉ có thể hoàn thành đơn hàng sau khi đã giao.",
                        Data = false
                    };
                }

                foreach (var item in orderItems)
                {
                    var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                    product.QuantitySold = product.QuantitySold + item.Quantity;
                    _unitOfWork.productRepository.Update(product);
                    var amountRefundArtisan = Math.Round((item.UnitPrice * item.Quantity) * 0.95m);
                    await _walletService.CreatePendingTransactionAsync(product.Artisan_id, amountRefundArtisan, 3);

                    getWalletSystem.PendingBalance = getWalletSystem.PendingBalance - amountRefundArtisan;

                    var addMoneyToWalletSystem = new WalletTransaction
                    {
                        Wallet_Id = getWalletSystem.Id,
                        Amount = amountRefundArtisan,
                        Type = WalletTransactionTypeEnum.Purchase,
                        Description = @$"Hoàn trả tiền đơn hàng ""{order.Id}"" cho nghệ nhân có mức giá {amountRefundArtisan}VND.",
                        CreationDate = DateTime.UtcNow.AddHours(7),
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        IsDeleted = false
                    };
                    await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);
                }

                var amountRefunDeliverySystem = (decimal)(order.Delivery_Amount * 0.85); 
                getWalletSystem.PendingBalance = getWalletSystem.PendingBalance - amountRefunDeliverySystem;
                if(getWalletSystem.PendingBalance > 0)
                {
                    getWalletSystem.AvailableBalance = getWalletSystem.AvailableBalance + getWalletSystem.PendingBalance;
                    getWalletSystem.PendingBalance = 0;
                }
                
                _unitOfWork.walletRepository.Update(getWalletSystem);

                var addTotalAfterDeductions = new WalletTransaction
                {
                    Wallet_Id = getWalletSystem.Id,
                    Amount = amountRefunDeliverySystem,
                    Type = WalletTransactionTypeEnum.Purchase,
                    Description = @$"Hoàn trả tiền phí vận chuyển của đơn hàng ""{order.Id}"" cho đơn vị vận chuyển với mức giá {amountRefunDeliverySystem}VND.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                await _unitOfWork.walletTransactionRepository.AddAsync(addTotalAfterDeductions);
            }
            order.Status = statusDto;
            order.ModificationDate = DateTime.UtcNow.AddHours(7);
            foreach (var item in orderItems)
            {
                item.Status = statusDto;
                item.ModificationDate = DateTime.UtcNow.AddHours(7);
                _unitOfWork.orderItemRepository.Update(item);
            }

            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>()
            {
                Error = 0,
                Message = "Cập nhật trạng thái đơn hàng thành công",
                Data = true
            };
        }

        public async Task<Result<int>> CountOrdersByUserIdAsync(Guid userId)
        {
            var totalOrders = await _unitOfWork.orderRepository.CountAsyncForUser(userId);

            return new Result<int>
            {
                Error = 0,
                Message = "Đếm số đơn hàng theo artisan và trạng thái thành công",
                Data = totalOrders
            };
        }

        public async Task<Result<OrderCountDto>> CountOrdersByArtisanIdAsync(Guid artisanId)
        {
            var totalOrders = await _unitOfWork.orderRepository.CountAsyncForArtisan(artisanId);
            var statusCounts = await _unitOfWork.orderRepository.GetStatusCountsAsyncForArtisan(artisanId);

            var result = new OrderCountDto
            {
                TotalOrders = totalOrders,
                CompletedOrders = statusCounts.GetValueOrDefault(OrderStatusEnum.Completed.ToString(), 0),
                CancelledOrders = statusCounts.GetValueOrDefault(OrderStatusEnum.Cancelled.ToString(), 0),
                RefundedOrders = statusCounts.GetValueOrDefault(OrderStatusEnum.Refunded.ToString(), 0),
                RejectedOrders = statusCounts.GetValueOrDefault(OrderStatusEnum.Rejected.ToString(), 0),
                DeliveryFailedOrders = statusCounts.GetValueOrDefault(OrderStatusEnum.DeliveryFailed.ToString(), 0)
            };

            return new Result<OrderCountDto>()
            {
                Error = 0,
                Message = "Đếm số đơn hàng theo artisan và trạng thái thành công",
                Data = result
            };
        }

        //Dashboard
        public async Task<Result<OrderDashboardForArtisanDto>> GetDashboardForArtisan(RevenueFilterDto filter)
        {
            var now = DateTime.UtcNow.AddHours(7);
            DateTime? from = null, to = null;

            switch (filter.Type)
            {
                case RevenueFilterType.Day:
                    from = now.Date;
                    to = now.Date.AddDays(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Week:
                    var startOfWeek = now.AddDays(-(int)now.DayOfWeek).Date.AddHours(7);
                    from = startOfWeek;
                    to = startOfWeek.AddDays(7).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Month:
                    from = new DateTime(now.Year, now.Month, 1).AddHours(7);
                    to = from.Value.AddMonths(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Year:
                    from = new DateTime(now.Year, 1, 1).AddHours(7);
                    to = from.Value.AddYears(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Custom:
                    from = filter.From;
                    to = filter.To;
                    break;

                default:
                    throw new ArgumentException("Filter type không hợp lệ.");
            }

            decimal totalRevenue = await _unitOfWork.orderRepository.SumRevenueForArtisanAsync(filter.ArtisanId, from, to);
            var totalOrders = await _unitOfWork.orderRepository.CountAsyncForArtisan(filter.ArtisanId, from, to);
            var statusCounts = await _unitOfWork.orderRepository.GetStatusCountsAsyncForArtisan(filter.ArtisanId, from, to);

            return new Result<OrderDashboardForArtisanDto>
            {
                Error = 0,
                Message = "Lấy dữ liệu dashboard thành công.",
                Data = new OrderDashboardForArtisanDto
                {
                    TotalOrders = totalOrders,
                    TotalRevenue = totalRevenue,
                    OrderStatusCounts = statusCounts
                }
            };
        }

        public async Task<Result<OrderDashboardForAdminDto>> GetDashboardForAdmin(RevenueFilterForAdmin filter)
        {
            var now = DateTime.UtcNow.AddHours(7);
            DateTime? from = null, to = null;

            switch (filter.Type)
            {
                case RevenueFilterType.Day:
                    from = now.Date;
                    to = now.Date.AddDays(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Week:
                    var startOfWeek = now.AddDays(-(int)now.DayOfWeek).Date.AddHours(7);
                    from = startOfWeek;
                    to = startOfWeek.AddDays(7).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Month:
                    from = new DateTime(now.Year, now.Month, 1).AddHours(7);
                    to = from.Value.AddMonths(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Year:
                    from = new DateTime(now.Year, 1, 1).AddHours(7);
                    to = from.Value.AddYears(1).AddTicks(-1).AddHours(7);
                    break;

                case RevenueFilterType.Custom:
                    from = filter.From;
                    to = filter.To;
                    break;

                default:
                    throw new ArgumentException("Filter type không hợp lệ.");
            }

            decimal totalRevenueBeforeFee = await _unitOfWork.orderRepository.SumRevenueForAdminBeforFeeAsync(from, to);
            decimal totalRevenueAfterFee = await _unitOfWork.orderRepository.SumRevenueForAdminAfterFeeAsync(from, to);
            decimal totalRevenueDeliveryFee = await _unitOfWork.orderRepository.SumRevenueForAdminDeliveryFeeAsync(from, to);
            decimal totalRevenueProductFee = await _unitOfWork.orderRepository.SumRevenueForAdminProductFeeAsync(from, to);
            var totalOrders = await _unitOfWork.orderRepository.CountAsyncForAdmin(from, to);
            var statusCounts = await _unitOfWork.orderRepository.GetStatusCountsAsyncForAdmin(from, to);

            return new Result<OrderDashboardForAdminDto>
            {
                Error = 0,
                Message = "Lấy dữ liệu dashboard thành công.",
                Data = new OrderDashboardForAdminDto
                {
                    TotalOrders = totalOrders,
                    TotalRevenueBeforeFee = totalRevenueBeforeFee,
                    TotalRevenueDeliveryFee = totalRevenueDeliveryFee,
                    TotalRevenueProductFee = totalRevenueProductFee,
                    TotalRevenueAfterFee = totalRevenueAfterFee,
                    OrderStatusCounts = statusCounts
                }
            };
        }

        public async Task<Result<ProductCountByMonthDto>> GetProductCountsByMonthAsync(int year, Guid? artisanId = null)
        {
            if (year < 1900 || year > DateTime.UtcNow.AddHours(7).Year)
            {
                return new Result<ProductCountByMonthDto>
                {
                    Error = 1,
                    Message = "Năm không hợp lệ."
                };
            }

            var data = await _unitOfWork.orderRepository.GetProductCountsByMonthAsync(year, artisanId);

            return new Result<ProductCountByMonthDto>
            {
                Error = 0,
                Message = "Lấy dữ liệu biểu đồ thành công.",
                Data = data
            };
        }

        public async Task AutoCompleteOrderItemsAsync()
        {
            var getOrderItems = await _unitOfWork.orderItemRepository.GetOrderItemsWithDeliveryStatusAsync();
            foreach (var item in getOrderItems)
            {
                item.Status = OrderStatusEnum.Completed;
                item.ModificationDate = DateTime.UtcNow.AddHours(7);
                _unitOfWork.orderItemRepository.Update(item);

                var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(item.OrderId);
                if (order != null)
                {
                    bool allCompleted = order.OrderItems.All(oi => oi.Status == OrderStatusEnum.Completed);
                    if (allCompleted)
                    {
                        order.Status = OrderStatusEnum.Completed;
                        order.ModificationDate = DateTime.UtcNow.AddHours(7);
                        _unitOfWork.orderRepository.Update(order);
                    }
                }
            }

            await _unitOfWork.SaveChangeAsync();
        }
    }
}
