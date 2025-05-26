using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGP.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Category.Include(x => x.SubCategories).ToListAsync();
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            return await _context.Category.Include(x => x.SubCategories).Where(p => p.Id == categoryId).FirstOrDefaultAsync();
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Category.Include(x => x.SubCategories).Where(p => p.CategoryName == name).FirstOrDefaultAsync();
        }
    }
}
