using CGP.Application.Common;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IFavouriteRepository : IGenericRepository<Favourite>
    {
        public Task<List<Favourite>> GetFavouritesByUserId(Guid id);
        public Task AddFavourite(Favourite favourite);
        public Task<Favourite> GetFavouriteByUserAndProduct(Guid userId, Guid productId);
        public Task DeleteFavouriteByUserAndProduct(Guid userId, Guid productId);
        public Task<bool> CheckFavourite(Guid userId, Guid productId);
    }
}
