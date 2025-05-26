using AutoMapper;
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
    public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
    {
        private readonly AppDbContext _context;

        public SubCategoryRepository(AppDbContext dataContext, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public void DeleteSubCategory(SubCategory SubCategory)
        {
            _context.Remove(SubCategory);
        }

        public async Task<ICollection<SubCategory>> GetSubCategories()
        {
            return await _context.SubCategory.OrderBy(p => p.CreationDate).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(Guid id)
        {
            return await _context.SubCategory.Include(sc => sc.Category).FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public void UpdateSubCategory(SubCategory SubCategory)
        {
            _context.Update(SubCategory);
        }

        public async Task<SubCategory> GetSubCategoryByName(string name)
        {
            var result = await _context.SubCategory.Where(p => p.SubName == name).FirstOrDefaultAsync();
            return result;
        }
    }
}
