using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        public Task<ICollection<SubCategory>> GetSubCategories();
        public Task<List<SubCategory>> GetSubCategoriesByArtisanIdAsync(Guid artisanId);
        public Task<SubCategory> GetSubCategoryById(Guid id);
        public Task<SubCategory> GetSubCategoryByName(string name);
        public void DeleteSubCategory(SubCategory SubCategory);
    }
}
