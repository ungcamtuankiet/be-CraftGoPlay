using CGP.Application;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderItemRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> GetOrderItemsByIdAsync(Guid orderItemId)
        {
            return await _dbContext.OrderItem
                .Include(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.User)
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Payment)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId)
        {
            return await _dbContext.OrderItem
                .Include(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(oi =>oi.Product)
                .ThenInclude(p => p.User)    
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsWithDeliveryStatusAsync()
        {
            var now = DateTime.UtcNow.AddHours(7);
            var orderItems = await _dbContext.OrderItem
                    .Where(oi => oi.Status == OrderStatusEnum.Delivered && oi.ModificationDate != null && oi.ModificationDate <= now.AddMinutes(-3))
                    .ToListAsync();
            return orderItems;
        }
    }
}
