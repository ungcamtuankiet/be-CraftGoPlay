using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayoutService _payoutService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IOrderService _orderService;
        private static string FOLDER = "return-request";

        public ReturnRequestService(IUnitOfWork unitOfWork, IMapper mapper, IPayoutService payoutService, ICloudinaryService cloudinaryService, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payoutService = payoutService;
            _cloudinaryService = cloudinaryService;
            _orderService = orderService;
        }

        public async Task<Result<object>> GetReturnRequestByUserIdAsync(Guid userId, ReturnStatusEnum? status, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewReturnRequestDTO>>(await _unitOfWork.returnRequestRepository.GetByUserIdAsync(userId, pageIndex, pageSize, status));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách yêu cầu hoàn trả hàng thành công.",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<object>> GetReturnRequestByArtisanIdAsync(Guid artisanId, ReturnStatusEnum? status, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewReturnRequestDTO>>(await _unitOfWork.returnRequestRepository.GetByArtisanIdAsync(artisanId, pageIndex, pageSize, status));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách yêu cầu hoàn trả hàng thành công.",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<object>> GetEscalatedReturnRequestAsync(int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewEscalatedDTO>>(await _unitOfWork.returnRequestRepository.GetEscalatedBAsync(pageIndex, pageSize));
            return new Result<object>
            {
                Error = 0,
                Message = "Lấy danh sách khiếu nại hoàn trả hàng thành công.",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<bool>> RefundOrderAsync(SendRefundRequestDTO dto)
        {
            var orderItem = await _unitOfWork.orderItemRepository.GetOrderItemsByIdAsync(dto.OrderItemId);
            var returnRequest = _mapper.Map<ReturnRequest>(dto);
            var checkReturnRequest = await _unitOfWork.returnRequestRepository.GetReturnRequestByOrderItemIdAsync(dto.OrderItemId);
            var uploadResult = await _cloudinaryService.UploadProductImage(dto.ImageUrl, FOLDER);
            var order = await _unitOfWork.orderRepository.GetByIdAsync(orderItem.OrderId);

            // Kiểm tra yêu cầu hoàn trả đã tồn tại
            if (checkReturnRequest != null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn trả hàng đã tồn tại.",
                    Data = false
                };
            }

            // Kiểm tra trạng thái đơn hàng
            if (orderItem.Status == OrderStatusEnum.Completed)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng đã được xác nhận là nhận đơn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            if (orderItem.Status != OrderStatusEnum.Delivered)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng chưa được giao đến tay bạn nên không thể hoàn hàng.",
                    Data = false
                };
            }

            // Kiểm tra đơn hàng và thanh toán
            if (orderItem == null || orderItem.Order.Payment == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng không tồn tại hoặc chưa thanh toán.",
                    Data = false
                };
            }

            if (orderItem.Order.Payment.IsRefunded)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Đơn hàng đã được hoàn tiền trước đó.",
                    Data = false
                };
            }

            // Kiểm tra hình ảnh
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.SecureUrl.ToString()))
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Tải lên hình ảnh hoàn hàng thất bại.",
                    Data = false
                };
            }

            if (dto.ImageUrl == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Vui lòng cung cấp hình ảnh sản phẩm để hoàn hàng.",
                    Data = false
                };
            }

            // Thêm yêu cầu hoàn trả
            returnRequest.ImageUrl = uploadResult.SecureUrl.ToString();
            returnRequest.ModificationDate = DateTime.UtcNow.AddHours(7);
            await _unitOfWork.returnRequestRepository.AddAsync(returnRequest);

            // Cập nhật trạng thái OrderItem
            orderItem.Status = OrderStatusEnum.ReturnRequested;

            // Lấy tất cả OrderItem của Order
            var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(orderItem.OrderId);

            // Kiểm tra xem tất cả OrderItem có trạng thái ReturnRequested không
            bool allItemsReturned = orderItems.All(item => item.Status == OrderStatusEnum.ReturnRequested);

            // Cập nhật trạng thái Order
            if (allItemsReturned)
            {
                order.Status = OrderStatusEnum.FullReturn;
            }
            else
            {
                order.Status = OrderStatusEnum.PartialReturn;
            }

            _unitOfWork.orderRepository.Update(order);

            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = 0,
                Message = "Đã gửi yêu cầu hoàn trả hàng thành công.",
                Data = true
            };
        }

        public async Task<Result<object>> UpdateStatusReturnRequestAsync(Guid returnRequestId, ReturnStatusEnum status, RejectReturnReasonEnum rejectReason)
        {
            var getRequest = await _unitOfWork.returnRequestRepository.GetReturnRequestById(returnRequestId);
            var getWalletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(getRequest.UserId);
            var getWalletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền không tồn tại."
                };
            }

            if (getRequest.Status != ReturnStatusEnum.Pending)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền đã được xử lý trước đó."
                };
            }

            if (getRequest.Status == ReturnStatusEnum.Approved || getRequest.Status == ReturnStatusEnum.Rejected || getRequest.Status == ReturnStatusEnum.Refunded)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn tiền đã được duyệt trước đó."
                };
            }

            if (status == ReturnStatusEnum.Approved)
            {
                getRequest.RejectReturnReasonEnum = null;

                // Lấy thông tin đơn hàng và các mục đơn hàng
                var order = await _unitOfWork.orderRepository.GetByIdAsync(getRequest.OrderItem.OrderId);
                var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);

                // Kiểm tra nếu đơn hàng đã được hoàn tiền
                if (order.Payment.IsRefunded)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Đơn hàng đã được hoàn tiền trước đó."
                    };
                }

                // Tính tổng số tiền hoàn trả
                decimal refundAmount = 0;

                // Lấy danh sách yêu cầu hoàn trả của đơn hàng
                var returnRequests = await _unitOfWork.returnRequestRepository.GetReturnRequestsByOrderIdAsync(order.Id);
                var approvedReturnRequests = returnRequests.Where(r => r.Status == ReturnStatusEnum.Approved || r.Status == ReturnStatusEnum.Refunded).ToList();
                bool isLastItemToApprove = approvedReturnRequests.Count + 1 == orderItems.Count && returnRequests.Any(r => r.Id == returnRequestId && r.Status == ReturnStatusEnum.Pending);

                // Kiểm tra số lượng sản phẩm trong đơn hàng
                if (orderItems.Count == 1)
                {
                    // Nếu đơn hàng chỉ có 1 sản phẩm, hoàn trả toàn bộ số tiền (bao gồm phí vận chuyển)
                    refundAmount = (decimal)(order.Product_Amount - order.ProductDiscount);
                    order.Status = OrderStatusEnum.FullReturn;
                    order.Payment.IsRefunded = true;
                }
                else
                {
                    // Nếu đơn hàng có nhiều sản phẩm
                    if (isLastItemToApprove)
                    {
                        // Nếu đây là sản phẩm cuối cùng được duyệt hoàn trả
                        // Tính tổng số tiền sản phẩm đã hoàn trước đó
                        decimal previouslyRefundedAmount = approvedReturnRequests.Sum(r =>
                            (decimal)(r.OrderItem.UnitPrice * r.OrderItem.Quantity) - (decimal)(order.ProductDiscount / order.OrderItems.Count));

                        // Chỉ hoàn số tiền sản phẩm còn lại (không bao gồm phí vận chuyển)
                        decimal totalProductAmount = (decimal)(order.Product_Amount - order.ProductDiscount) - (decimal)(order.ProductDiscount / order.OrderItems.Count);
                        refundAmount = totalProductAmount - previouslyRefundedAmount;
                        order.Status = OrderStatusEnum.FullReturn;
                        order.Payment.IsRefunded = true;
                    }
                    else
                    {
                        // Nếu chỉ một phần sản phẩm được hoàn trả, chỉ hoàn tiền sản phẩm hiện tại
                        refundAmount = (decimal)(getRequest.OrderItem.UnitPrice * getRequest.OrderItem.Quantity) - (decimal)(order.ProductDiscount / order.OrderItems.Count);
                        order.Status = OrderStatusEnum.PartialReturn;
                    }
                }

                // Cập nhật trạng thái OrderItem thành Refunded
                getRequest.OrderItem.Status = OrderStatusEnum.Refunded;
                _unitOfWork.orderItemRepository.Update(getRequest.OrderItem);

                // Kiểm tra trạng thái của tất cả OrderItem để cập nhật Order status thành Completed
                var allItemsCompleted = orderItems.All(item => item.Status == OrderStatusEnum.Completed || item.Status == OrderStatusEnum.Refunded);
                if (allItemsCompleted)
                {
                    order.Status = OrderStatusEnum.Completed;
                }

                // Cập nhật ví hệ thống
                getWalletSystem.PendingBalance -= refundAmount;
                var addMoneyToWalletSystem = new WalletTransaction
                {
                    Wallet_Id = getWalletSystem.Id,
                    Amount = refundAmount,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Hoàn trả tiền cho sản phẩm ""{getRequest.OrderItemId}"" của đơn hàng ""{getRequest.OrderItem.Order.Id}"".",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                _unitOfWork.walletRepository.Update(getWalletSystem);
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);

                // Cập nhật ví người dùng
                getWalletUser.AvailableBalance += refundAmount;
                var addMoneyToWalletUser = new WalletTransaction
                {
                    Wallet_Id = getWalletUser.Id,
                    Amount = refundAmount,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Sản phẩm ""{getRequest.OrderItemId}"" của đơn hàng ""{getRequest.OrderItem.OrderId}"" được hoàn tiền.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                _unitOfWork.walletRepository.Update(getWalletUser);
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletUser);

                // Cập nhật trạng thái đơn hàng
                _unitOfWork.orderRepository.Update(order);
            }

            if (status == ReturnStatusEnum.Rejected)
            {
                getRequest.RejectReturnReasonEnum = rejectReason;
            }

            getRequest.Status = status;
            getRequest.ApprovedAt = DateTime.UtcNow.AddHours(7);
            getRequest.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.returnRequestRepository.Update(getRequest);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Cập nhật trạng thái yêu cầu hoàn tiền thành công.",
                Data = getRequest
            };
        }

        public async Task<Result<object>> EscalateReturnRequestAsync(Guid returnRequestId, string reason)
        {
            var request = await _unitOfWork.returnRequestRepository.GetByIdAsync(returnRequestId);

            if (request == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu hoàn trả không tồn tại."
                };
            }

            if (request.Status != ReturnStatusEnum.Rejected)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Chỉ có thể khiếu nại khi yêu cầu đã bị từ chối."
                };
            }

            request.Status = ReturnStatusEnum.Escalated;
            request.ModificationDate = DateTime.UtcNow.AddHours(7);
            request.Description = reason;

            _unitOfWork.returnRequestRepository.Update(request);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Đã gửi khiếu nại lên hệ thống. Admin sẽ xem xét.",
                Data = request
            };
        }

        public async Task<Result<object>> ResolveEscalatedRequestAsync(Guid returnRequestId, bool acceptRefund)
        {
            var request = await _unitOfWork.returnRequestRepository.GetReturnRequestById(returnRequestId);
            var walletUser = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(request.UserId);

            if (request == null || request.Status != ReturnStatusEnum.Escalated)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy yêu cầu khiếu nại hoặc yêu cầu không hợp lệ."
                };
            }

            if (acceptRefund)
            {

                var order = request.OrderItem.Order;
                var orderItems = await _unitOfWork.orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);

                if (order.Payment.IsRefunded)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Đơn hàng đã được hoàn tiền trước đó."
                    };
                }

                decimal refundAmount = 0;

                var returnRequests = await _unitOfWork.returnRequestRepository.GetReturnRequestsByOrderIdAsync(order.Id);
                var approvedReturnRequests = returnRequests.Where(r => r.Status == ReturnStatusEnum.Approved || r.Status == ReturnStatusEnum.Refunded).ToList();
                bool isLastItemToApprove = approvedReturnRequests.Count + 1 == orderItems.Count;

                if (orderItems.Count == 1)
                {
                    refundAmount = (decimal)(order.Product_Amount - order.ProductDiscount);
                    order.Status = OrderStatusEnum.FullReturn;
                    order.Payment.IsRefunded = true;
                }
                else
                {
                    // Nếu đơn hàng có nhiều sản phẩm
                    if (isLastItemToApprove)
                    {
                        // Nếu đây là sản phẩm cuối cùng được duyệt hoàn trả
                        // Tính tổng số tiền sản phẩm đã hoàn trước đó
                        decimal previouslyRefundedAmount = approvedReturnRequests.Sum(r =>
                            (decimal)(r.OrderItem.UnitPrice * r.OrderItem.Quantity) - (decimal)(order.ProductDiscount / order.OrderItems.Count));

                        // Chỉ hoàn số tiền sản phẩm còn lại (không bao gồm phí vận chuyển)
                        decimal totalProductAmount = (decimal)(order.Product_Amount - order.ProductDiscount) - (decimal)(order.ProductDiscount / order.OrderItems.Count);
                        refundAmount = totalProductAmount - previouslyRefundedAmount;
                        order.Status = OrderStatusEnum.FullReturn;
                        order.Payment.IsRefunded = true;
                    }
                    else
                    {
                        // Nếu chỉ một phần sản phẩm được hoàn trả, chỉ hoàn tiền sản phẩm hiện tại
                        refundAmount = (decimal)(request.OrderItem.UnitPrice * request.OrderItem.Quantity) - (decimal)(order.ProductDiscount / order.OrderItems.Count);
                        order.Status = OrderStatusEnum.PartialReturn;
                    }
                }

                request.Status = ReturnStatusEnum.Refunded;
                request.ApprovedAt = DateTime.UtcNow.AddHours(7);
                request.IsRefunded = true;

                request.OrderItem.Status = OrderStatusEnum.Refunded;
                _unitOfWork.orderItemRepository.Update(request.OrderItem);

                var allItemsCompleted = orderItems.All(item => item.Status == OrderStatusEnum.Completed || item.Status == OrderStatusEnum.Refunded);
                if (allItemsCompleted)
                {
                    order.Status = OrderStatusEnum.Completed;
                }

                _unitOfWork.orderRepository.Update(order);

                var walletSystem = await _unitOfWork.walletRepository.GetWalletSystem();

                walletSystem.PendingBalance -= refundAmount;
                var addMoneyToWalletSystem = new WalletTransaction
                {
                    Wallet_Id = walletSystem.Id,
                    Amount = refundAmount,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Hoàn trả tiền cho sản phẩm ""{request.OrderItemId}"" của đơn hàng ""{order.Id}"".",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                _unitOfWork.walletRepository.Update(walletSystem);
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletSystem);

                walletUser.AvailableBalance += refundAmount;
                var addMoneyToWalletUser = new WalletTransaction
                {
                    Wallet_Id = walletUser.Id,
                    Amount = refundAmount,
                    Type = WalletTransactionTypeEnum.Refund,
                    Description = @$"Sản phẩm ""{request.OrderItemId}"" của đơn hàng ""{order.Id}"" được hoàn tiền.",
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                };
                _unitOfWork.walletRepository.Update(walletUser);
                await _unitOfWork.walletTransactionRepository.AddAsync(addMoneyToWalletUser);
            }
            else
            {
                request.Status = ReturnStatusEnum.Resolved;
                request.OrderItem.Order.Status = OrderStatusEnum.Completed;
            }

            request.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.returnRequestRepository.Update(request);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Đã xử lý khiếu nại.",
                Data = request
            };
        }
    }
}
