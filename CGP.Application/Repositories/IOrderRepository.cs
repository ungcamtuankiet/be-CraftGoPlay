using CGP.Contract.DTO.DashBoard;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetOrdersByTransactionIdAsync(Guid transactionId);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId, int pageIndex, int pageSize, OrderStatusEnum? status);
        Task<List<Order>> GetListOrderAsync(int pageIndex, int pageSize, OrderStatusEnum? status);
        Task<List<Order>> GetOrdersByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, OrderStatusEnum? status);

        //Dashboard
        Task<int> CountAsyncForArtisan(Guid artisanId, DateTime? from = null, DateTime? to = null);
        Task<int> CountAsyncForAdmin(DateTime? from = null, DateTime? to = null);
        Task<Dictionary<string, int>> GetStatusCountsAsyncForArtisan(Guid artisanId, DateTime? from = null, DateTime? to = null);
        Task<Dictionary<string, int>> GetStatusCountsAsyncForAdmin(DateTime? from = null, DateTime? to = null);
        Task<decimal> SumRevenueForArtisanAsync(Guid artisanId, DateTime? from = null, DateTime? to = null);
        Task<decimal> SumRevenueForAdminBeforFeeAsync(DateTime? from = null, DateTime? to = null);
        Task<decimal> SumRevenueForAdminDeliveryFeeAsync(DateTime? from = null, DateTime? to = null);
        Task<decimal> SumRevenueForAdminProductFeeAsync(DateTime? from = null, DateTime? to = null);
        Task<decimal> SumRevenueForAdminAfterFeeAsync(DateTime? from = null, DateTime? to = null);
        Task<ProductCountByMonthDto> GetProductCountsByMonthAsync(int year, Guid? artisanId = null);
    }
}
