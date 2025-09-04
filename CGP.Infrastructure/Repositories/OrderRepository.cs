using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.DashBoard;
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
                .OrderByDescending(o => o.ModificationDate)
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
                .OrderByDescending(o => o.ModificationDate)
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
                .OrderByDescending(o => o.ModificationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //Dashboard
        public async Task<int> CountAsyncForUser(Guid userId)
        {
            var query = _dbContext.Order
                .Where(o => o.UserId == userId);

            return await query.CountAsync();
        }

        public async Task<int> CountAsyncForArtisan(Guid artisanId, DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.Order
                .Where(o => o.OrderItems.Any(oi => oi.ArtisanId == artisanId));

            if (from.HasValue)
                query = query.Where(o => o.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreationDate <= to.Value);

            return await query.CountAsync();
        }

        public async Task<int> CountAsyncForAdmin(DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.Order.AsQueryable();

            if (from.HasValue)
                query = query.Where(o => o.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreationDate <= to.Value);

            return await query.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetStatusCountsAsyncForArtisan(Guid artisanId, DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.Order
                .Where(o => o.OrderItems.Any(oi => oi.ArtisanId == artisanId));

            if (from.HasValue)
                query = query.Where(o => o.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreationDate <= to.Value);

            return await query
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetStatusCountsAsyncForAdmin(DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.Order.AsQueryable();

            if (from.HasValue)
                query = query.Where(o => o.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreationDate <= to.Value);

            return await query
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
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

            // Fix: Remove ?? 0 (not needed for decimal), and close parenthesis for Math.Round
            return await query.SumAsync(i => Math.Round((i.UnitPrice * i.Quantity) * 0.95m));
        }


        private async Task<decimal> SumRevenueForAdminAsync(Expression<Func<OrderItem, decimal?>> selector, DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.OrderItem
                .Where(i => i.Status == OrderStatusEnum.Completed);

            if (from.HasValue)
                query = query.Where(i => i.Order.CreationDate >= from.Value);

            if (to.HasValue)
                query = query.Where(i => i.Order.CreationDate <= to.Value);

            return await query.SumAsync(selector) ?? 0;
        }


        public Task<decimal> SumRevenueForAdminBeforFeeAsync(DateTime? from = null, DateTime? to = null)
        {
            return SumRevenueForAdminAsync(i => (decimal?)i.Order.TotalPrice, from, to);
        }

        public Task<decimal> SumRevenueForAdminDeliveryFeeAsync(DateTime? from = null, DateTime? to = null)
        {
            return SumRevenueForAdminAsync(
                i => (decimal?)i.Order.Delivery_Amount * 0.15m,
                from,
                to
            );
        }

        public Task<decimal> SumRevenueForAdminProductFeeAsync(DateTime? from = null, DateTime? to = null)
        {
            return SumRevenueForAdminAsync(
                i => (decimal?)i.Order.Product_Amount * 0.05m,
                from,
                to
            );
        }

        public Task<decimal> SumRevenueForAdminAfterFeeAsync(DateTime? from = null, DateTime? to = null)
        {
            return SumRevenueForAdminAsync(
                i => (decimal?)i.Order.TotalPrice
                    - (decimal?)i.Order.Delivery_Amount * 0.85m
                    - (decimal?)i.Order.DeliveryDiscount
                    - (decimal?)i.Order.Product_Amount * 0.95m
                    - (decimal?)i.Order.ProductDiscount,
                from,
                to
            );
        }

        public async Task<ProductCountByMonthDto> GetProductCountsByMonthAsync(int year, Guid? artisanId = null)
        {
            // Initialize lists for 12 months (Jan-Dec)
            var availableProducts = new List<int>(new int[12]);
            var soldProducts = new List<int>(new int[12]);

            // Query for available products (QuantityInStock > 0)
            var availableQuery = _dbContext.Product
                .Where(p => p.QuantitySold > 0 && p.CreationDate.Year <= year);

            if (artisanId.HasValue)
            {
                availableQuery = availableQuery.Where(p => p.Artisan_id == artisanId.Value);
            }

            var availableProductsByMonth = await availableQuery
                .GroupBy(p => p.CreationDate.Month - 1) // 0-based index for months (0 = Jan, 11 = Dec)
                .Select(g => new { MonthIndex = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var item in availableProductsByMonth)
            {
                availableProducts[item.MonthIndex] = item.Count;
            }

            // Query for sold products (OrderItems with Status == Completed)
            var soldQuery = _dbContext.OrderItem
                .Where(oi => oi.Status == OrderStatusEnum.Completed && oi.Order.CreationDate.Year == year);

            if (artisanId.HasValue)
            {
                soldQuery = soldQuery.Where(oi => oi.ArtisanId == artisanId.Value);
            }

            var soldProductsByMonth = await soldQuery
                .GroupBy(oi => oi.Order.CreationDate.Month - 1) // 0-based index for months
                .Select(g => new { MonthIndex = g.Key, Count = g.Sum(oi => oi.Quantity) })
                .ToListAsync();

            foreach (var item in soldProductsByMonth)
            {
                soldProducts[item.MonthIndex] = item.Count;
            }

            return new ProductCountByMonthDto
            {
                AvailableProducts = availableProducts,
                SoldProducts = soldProducts,
                Year = year
            };
        }
    }
}
