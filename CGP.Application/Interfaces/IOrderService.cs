using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.Order;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersAsync(int pageIndex, int pageSize, OrderStatusEnum? status);
        Task<Result<ViewOrderDTO>> GetOrderByIdAssync(Guid id);
        Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersByUserIdAsync(Guid userId, int pageIndex, int pageSize, OrderStatusEnum? status);
        Task<ResponseOrder<List<ViewOrderDTO>>> GetOrdersByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, OrderStatusEnum? status);
        Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto statusDto);
        Task<Result<Guid>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, double Delivery_Amount, Guid address, PaymentMethodEnum paymentMethod);
        Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, Guid address, double Delivery_Amount, CreateDirectOrderDto dto);
        Task<Result<string>> CreateVnPayUrlAsync(Guid transactionId, HttpContext httpContext);
        Task<Result<object>> HandleVnPayReturnAsync(IQueryCollection query);
        Task<Result<bool>> RefundOrderAsync(SendRefundRequestDTO dto);
    }
}
