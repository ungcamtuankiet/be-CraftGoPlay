using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Rating;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewRatingDTO>>> GetRatingsByArtisanId(Guid artisanId, int pageIndex, int pageSize)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(artisanId);
            if (checkUser == null)
            {
                return new Result<List<ViewRatingDTO>>()
                {
                    Error = 1,
                    Message = "Nghệ nhân không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewRatingDTO>>(await _unitOfWork.ratingRepository.GetRatingsByArtisanIdAsync(artisanId, pageIndex, pageSize));
            return new Result<List<ViewRatingDTO>>()
            {
                Error = 0,
                Message = "Lấy đánh giá thành công.",
                Data = result
            };
        }

        public async Task<Result<List<ViewRatingDTO>>> GetRatingsByUserId(Guid userId, int pageIndex, int pageSize)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if(checkUser == null)
            {
                return new Result<List<ViewRatingDTO>>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewRatingDTO>>(await _unitOfWork.ratingRepository.GetRatingsByUserIdAsync(userId, pageIndex, pageSize));
            return new Result<List<ViewRatingDTO>>()
            {
                Error = 0,
                Message = "Lấy đánh giá thành công.",
                Data = result
            };
        }

        public async Task<Result<object>> RatingProduct(RatingDTO dto)
        {
            var checkHasPurchased = await _unitOfWork.ratingRepository.HasPurchased(dto.UserId, dto.ProductId);
            var rating = _mapper.Map<Rating>(dto);
            if (!checkHasPurchased)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Bạn chỉ có thể đánh giá sản phẩm đã mua và đơn hàng đã hoàn thành.",
                    Data = null
                };
            }

            var hasRated = await _unitOfWork.ratingRepository.CheckRated(dto.UserId, dto.ProductId);
            if (hasRated)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Bạn đã đánh giá sản phẩm này rồi.",
                    Data = null
                };
            }
            await _unitOfWork.ratingRepository.AddAsync(rating);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 1,
                Message = "Đánh giá thành công.",
                Data = rating
            };
        }
    }
}
