using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<ICollection<Category>> GetCategories();
        public Task<Category> GetCategoryById(Guid id);
        public void DeleteCategory(Category category);
        public Task<Category> GetCategoryByName(string name);
    }
}
