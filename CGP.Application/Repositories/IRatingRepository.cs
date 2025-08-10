using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(Guid userId, int pageIndex, int pageSize);
        Task<IEnumerable<Rating>> GetRatingsByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize);
        Task<IEnumerable<Rating>> GetRatingsByProductIdAsync(Guid productId, int pageIndex, int pageSize, int? star);
        Task<int> GetTotalRatingsByProductIdAsync(Guid productId);
        Task<bool> HasPurchased(Guid userId, Guid productId);
        Task<bool> CheckRated(Guid userId, Guid productId);
    }
}
