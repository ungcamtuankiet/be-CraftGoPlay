using CGP.Contract.DTO.Favourite;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IFavouriteService
    {
        public Task<Result<List<ViewFavouriteDTO>>> GetFavourites(Guid id);
        public Task<Result<object>> AddFavourite(CreateFavouriteDTO request);
        public Task<Result<object>> DeleteFavouriteByUserAndProduct(Guid userId, Guid productId);
        public Task<Result<bool>> CheckFavourite(Guid userId, Guid productId);
    }
}
