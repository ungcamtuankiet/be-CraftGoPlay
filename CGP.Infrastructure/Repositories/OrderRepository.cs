﻿using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Order>> GetListOrderAsync()
        {
            return await _dbContext.Order
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
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
               .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _dbContext.Order
               .Include(o => o.User)
               .Include(o => o.Payment)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .ThenInclude(o => o.ProductImages)
               .Where(o => o.UserId == userId)
               .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByArtisanIdAsync(Guid artisanId)
        {
            return await _dbContext.Order
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
                .Where(o => o.OrderItems.Any(oi => oi.ArtisanId == artisanId))
                .ToListAsync();
        }
    }
}
