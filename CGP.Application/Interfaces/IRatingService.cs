using CGP.Contract.DTO.Rating;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IRatingService
    {
        public Task<Result<object>> RatingProduct(RatingDTO rating);
        public Task<Result<List<ViewRatingDTO>>> GetRatingsByUserId(Guid userId, int pageIndex, int pageSize);
        public Task<Result<List<ViewRatingDTO>>> GetRatingsByArtisanId(Guid artisanId, int pageIndex, int pageSize);
    }
}
