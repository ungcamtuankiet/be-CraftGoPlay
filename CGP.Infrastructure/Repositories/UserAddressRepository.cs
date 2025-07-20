using CGP.Application.Interfaces;
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
    public class UserAddressRepository : GenericRepository<UserAddress>, IUserAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public UserAddressRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task AddNewAddress(UserAddress userAddress)
        {
            await _dbContext.UserAddress.AddAsync(userAddress);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAddress(UserAddress userAddress)
        {
            _dbContext.UserAddress.Update(userAddress);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAddress(UserAddress userAddress)
        {
            _dbContext.UserAddress.Remove(userAddress);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserAddress?> GetUserAddressById(Guid addressId)
        {
            return await _dbContext.UserAddress
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == addressId);
        }

        public async Task<List<UserAddress>> GetUserAddressesByUserId(Guid userId)
        {
            return await _dbContext.UserAddress
                .Include(x => x.User)
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .ToListAsync();
        }

        public Task<UserAddress?> GetDefaultAddressByUserId(Guid userId)
        {
            return _dbContext.UserAddress
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsDefault == true);
        }

    }
}
