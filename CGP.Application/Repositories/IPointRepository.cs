using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        public Task<List<Point>> GetPointsAllUserIdAsync();
        public Task<Point> GetPointsByUserId(Guid userId);
    }
}
