using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Favourite;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> AddFavourite(CreateFavouriteDTO request)
        {
            var favourite = _mapper.Map<Favourite>(request);
            await _unitOfWork.favouriteRepository.AddFavourite(favourite);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object> { 
                Error = 0,
                Message = "Thêm vào mục yêu thích thành công",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteFavourite(Guid id)
        {
            var getFavourite = await _unitOfWork.favouriteRepository.GetByIdAsync(id);
            if (getFavourite == null) {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại",
                    Data = null
                };
            };
            await _unitOfWork.favouriteRepository.DeleteFavourite(getFavourite);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Bỏ yêu thích thành công",
                Data = null
            };
        }

        public async Task<Result<List<ViewFavouriteDTO>>> GetFavourites(Guid id)
        {
            var favourites = await _unitOfWork.favouriteRepository.GetFavouritesByUserId(id);
            var result = _mapper.Map<List<ViewFavouriteDTO>>(favourites);
            return new Result<List<ViewFavouriteDTO>>
            {
                Error = 0,
                Message = "Lấy dánh sách yêu thích thành công",
                Data = result
            };
        }

        public async Task<Result<bool>> CheckFavourite(Guid userId, Guid productId)
        {
            var isFavorited = await _unitOfWork.favouriteRepository.CheckFavourite(userId, productId);
            return new Result<bool>
            {
                Error = 0,
                Message = "Kiểm tra thành công",
                Data = isFavorited
            };
        }
    }
}
