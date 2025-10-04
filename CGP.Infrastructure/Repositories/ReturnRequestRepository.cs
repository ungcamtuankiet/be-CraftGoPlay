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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CGP.Infrastructure.Repositories
{

    public class ReturnRequestRepository : GenericRepository<ReturnRequest>, IReturnRequestRepository
    {
        private readonly AppDbContext _dbContext;

        public ReturnRequestRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ReturnRequest>> GetAllAsync(int pageIndex, int pageSize, ReturnStatusEnum status)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .Include(rr => rr.User)
                .Where(rr => rr.Status == status && !rr.IsDeleted)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(rr => rr.CreationDate)
                .ToListAsync();
        }

        public async Task<ReturnRequest> GetReturnRequestById(Guid id)
        {
            return await _dbContext.ReturnRequest
                .Include(o => o.OrderItem)
                .ThenInclude(o => o.Order)
                .ThenInclude(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<ReturnRequest>> GetReturnRequestsByOrderIdAsync(Guid orderId)
        {
            return await _dbContext.ReturnRequest
                .Include(r => r.OrderItem)
                .Where(r => r.OrderItem.OrderId == orderId)
                .OrderByDescending(rr => rr.CreationDate)
                .ToListAsync();
        }

        public async Task<List<ReturnRequest>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, ReturnStatusEnum? status)
        {
            var result = _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.ProductImages)
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.User)
                .Include(rr => rr.User)
                .AsQueryable();

            if (status.HasValue)
            {
                result = result.Where(o => o.Status == status.Value && o.UserId == userId);
            }

            return await result
                .Where(rr => rr.UserId == userId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(rr => rr.ModificationDate)
                .ToListAsync();
        }

        public async Task<List<ReturnRequest>> GetByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, ReturnStatusEnum? status)
        {
            var result = _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.ProductImages)
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.User)
                .Include(rr => rr.User)
                .AsQueryable();

            if (status.HasValue)
            {
                result = result.Where(o => o.Status == status.Value && o.OrderItem.ArtisanId == artisanId);
            }

            return await result
                .Where(rr => rr.OrderItem.ArtisanId == artisanId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(rr => rr.ModificationDate)
                .ToListAsync();
        }

        public async Task<ReturnRequest> GetByOrderItemIdAsync(Guid orderItemId)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .Include(rr => rr.User)
                .FirstOrDefaultAsync(rr => rr.OrderItemId == orderItemId && !rr.IsDeleted);
        }

        public async Task<List<ReturnRequest>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, ReturnStatusEnum status)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .Include(rr => rr.User)
                .Where(rr => rr.Status == status && !rr.IsDeleted && rr.UserId == userId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<ReturnRequest>> GetEscalatedBAsync(int pageIndex, int pageSize)
        {
            var result = await _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.ProductImages)
                .Include(rr => rr.OrderItem)
                .ThenInclude(rr => rr.Product)
                .ThenInclude(rr => rr.User)
                .Include(rr => rr.User)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(rr => rr.ModificationDate)
                .ToListAsync();

            return result;
                
        }

        public async Task<ReturnRequest> GetReturnRequestByOrderItemIdAsync(Guid orderItemId)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.OrderItem)
                .Include(rr => rr.User)
                .FirstOrDefaultAsync(rr => rr.OrderItemId == orderItemId && !rr.IsDeleted);
        }
    }
}
