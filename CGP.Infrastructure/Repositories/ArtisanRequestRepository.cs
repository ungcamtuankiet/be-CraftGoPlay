using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.ArtisanRequest;
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
    public class ArtisanRequestRepository : GenericRepository<ArtisanRequest>, IArtisanRequestRepository
    {
        private readonly AppDbContext _dbContext;

        public ArtisanRequestRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ArtisanRequest>> GetAllArtisanRequests()
        {
            return await _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .ToListAsync();
        }

        public async Task<ArtisanRequest> GetArtisanRequestById(Guid id)
        {
            return await _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ArtisanRequest>> GetRequestByStatus(int pageIndex, int pageSize, RequestArtisanStatus status)
        {
            var query =  _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .Where(r => r.Status == status);

            if (!string.IsNullOrWhiteSpace(status.ToString()))
            {
                switch (status.ToString().ToLower())
                {
                    case "pending":
                        query = query.Where(x => x.Status == RequestArtisanStatus.Pending);
                        break;
                    case "approved":
                        query = query.Where(x => x.Status == RequestArtisanStatus.Approved);
                        break;
                    case "rejected":
                        query = query.Where(x => x.Status == RequestArtisanStatus.Rejected);
                        break;
                    case "cancelled":
                        query = query.Where(x => x.Status == RequestArtisanStatus.Cancelled);
                        break;
                }
            }
            else
            {
                query = query.Where(x => x.Status == RequestArtisanStatus.Pending);
            }

            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task SendNewRequest(ArtisanRequest artisanRequest)
        {
            await _dbContext.ArtisanRequest.AddAsync(artisanRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ArtisanRequest?> GetPendingRequestByUserId(Guid userId)
        {
            return await _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .Where(r => r.UserId == userId && r.Status == RequestArtisanStatus.Pending)
                .FirstOrDefaultAsync();
        }


        public async Task CancelRequestByArtisan(ArtisanRequest artisanRequest)
        {
            artisanRequest.Status = RequestArtisanStatus.Cancelled;
            _dbContext.ArtisanRequest.Update(artisanRequest);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AcceptRequest(ArtisanRequest artisanRequestd)
        {
            artisanRequestd.Status = RequestArtisanStatus.Approved;
            _dbContext.ArtisanRequest.Update(artisanRequestd);
            await _dbContext.SaveChangesAsync();
        }


        public async Task RejectRequest(ArtisanRequest artisanRequest)
        {
            artisanRequest.Status = RequestArtisanStatus.Rejected;
            _dbContext.ArtisanRequest.Update(artisanRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ArtisanRequest?> GetLatestRequestByUserId(Guid userId)
        {
            return await _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Status)
                .FirstOrDefaultAsync();
        }
        public async Task<ArtisanRequest?> GetRequestByIdAndUserId(Guid requestId, Guid userId)
        {
            return await _dbContext.ArtisanRequest
                .Include(r => r.User)
                .Include(r => r.CraftVillages)
                .Include(r => r.CraftSkills)
                .FirstOrDefaultAsync(r => r.Id == requestId && r.UserId == userId);
        }
    }
}
