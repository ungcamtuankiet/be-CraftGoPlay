﻿using CGP.Contract.DTO.Order;
using CGP.Contract.DTO.Product;
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
        Task<Result<ViewOrderDTO>> GetOrderByIdAssync(Guid id);
        Task<Result<List<ViewOrderDTO>>> GetOrdersByUserIdAsync(Guid userId);
        Task<Result<List<Guid>>> CreateOrderFromCartAsync(Guid userId, List<Guid> selectedCartItemIds, PaymentMethodEnum paymentMethod);
        Task<Result<Guid>> CreateDirectOrderAsync(Guid userId, CreateDirectOrderDto dto);
        Task<Result<string>> CreateVnPayUrlAsync(Guid orderId, HttpContext httpContext);
        Task<Result<string>> HandleVnPayReturnAsync(IQueryCollection query);
    }
}
