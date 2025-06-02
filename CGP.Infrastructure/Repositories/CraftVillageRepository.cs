using CGP.Application.Common;
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
    public class CraftVillageRepository : GenericRepository<CraftVillage>, ICraftVillageRepository
    {
        private readonly AppDbContext _context;
        public CraftVillageRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public Task AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(List<Category> entities)
        {
            throw new NotImplementedException();
        }

        public Task CreateNewCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<CraftVillage>> GetAllCraftVillagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteCraftVillage(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SoftRemove(Category entity)
        {
            throw new NotImplementedException();
        }

        public void SoftRemoveRange(List<Category> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(List<Category> entities)
        {
            throw new NotImplementedException();
        }

        Task<List<Category>> IGenericRepository<Category>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Category?> IGenericRepository<Category>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Pagination<Category>> IGenericRepository<Category>.ToPagination(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
