using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetListOrderAsync(int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var query = _dbContext.Order
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(o => o.OrderAddress)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _dbContext.Order
               .Include(o => o.User)
               .Include(o => o.Payment)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .ThenInclude(o => o.ProductImages)
               .Include(o => o.OrderItems)
               .Include(o => o.User)
               .ThenInclude(o => o.ArtisanRequest)
               .Include(o => o.OrderAddress)
               .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByTransactionIdAsync(Guid transactionId)
        {
            return await _dbContext.Order
               .Include(o => o.User)
               .Include(o => o.Payment)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .ThenInclude(o => o.ProductImages)
               .Include(o => o.OrderAddress)
               .Where(o => o.TransactionId == transactionId)
               .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId, int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var query = _dbContext.Order
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.User)
                .Include(o => o.OrderAddress)
                .Where(o => o.UserId == userId)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<List<Order>> GetOrdersByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, OrderStatusEnum? status)
        {
            var query = _dbContext.Order
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.OrderAddress)
                .Where(o => o.OrderItems.Any(oi => oi.ArtisanId == artisanId))
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //Dashboard

        public async Task<int> CountAsyncForArtisan(Guid artisanId, Expression<Func<Order, bool>> predicate = null)
        {
            return await _dbContext.OrderItem
                .Where(i => i.ArtisanId == artisanId)
                .Select(i => i.OrderId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> CountAsyncForAdmin(Expression<Func<Order, bool>> predicate = null)
        {
            return await _dbContext.OrderItem
                .Select(i => i.OrderId)
                .Distinct()
                .CountAsync();
        }

        public async Task<Dictionary<string, int>> GetStatusCountsAsyncForArtisan(Guid artisanId)
        {
            return await _dbContext.OrderItem
                .Where(i => i.ArtisanId == artisanId)
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Select(x => x.OrderId).Distinct().Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetStatusCountsAsyncForAdmin()
        {
            return await _dbContext.OrderItem
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Select(x => x.OrderId).Distinct().Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        // Doanh thu trong khoảng thời gian
        public async Task<decimal> SumRevenueForArtisanAsync(Guid artisanId, DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.OrderItem
                .Where(i => i.ArtisanId == artisanId && i.Status == OrderStatusEnum.Completed);

            if (from.HasValue)
                query = query.Where(i => i.Order.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(i => i.Order.CreationDate <= to.Value);

            return await query.SumAsync(i => (decimal?)(i.UnitPrice * i.Quantity)) ?? 0;
        }

        public async Task<decimal> SumRevenueForAdminAsync(DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.OrderItem
                .Where(i => i.Status == OrderStatusEnum.Completed);

            if (from.HasValue)
                query = query.Where(i => i.Order.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(i => i.Order.CreationDate <= to.Value);

            return await query.SumAsync(i => (decimal?)(i.Order.TotalPrice)) ?? 0;
        }
    }
}
