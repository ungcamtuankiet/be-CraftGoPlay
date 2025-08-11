using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Rating;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
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

        public async Task<Result<List<ViewRatingProductDTO>>> GetRatingsByProductId(Guid productId, int pageIndex, int pageSize, int? star)
        {
            var checkProduct = await _unitOfWork.productRepository.GetByIdAsync(productId);
            if (checkProduct == null)
            {
                return new Result<List<ViewRatingProductDTO>>()
                {
                    Error = 1,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewRatingProductDTO>>(await _unitOfWork.ratingRepository.GetRatingsByProductIdAsync(productId, pageIndex, pageSize, star));

            return new Result<List<ViewRatingProductDTO>>()
            {
                Error = 0,
                Message = "Lấy đánh giá cho sản phẩm thành công.",
                Count = result.Count,
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
                Message = "Lấy đánh giá cho người dùng thành công.",
                Data = result
            };
        }

        public async Task<Result<object>> RatingProduct(RatingDTO dto)
        {
            var checkHasPurchased = await _unitOfWork.ratingRepository.HasPurchased(dto.UserId, dto.ProductId, dto.OrderItemId);
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

            var hasRated = await _unitOfWork.ratingRepository.CheckRated(dto.UserId, dto.OrderItemId);
            if (hasRated)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Bạn đã đánh giá sản phẩm này rồi.",
                    Data = null
                };
            }
            rating.ProductId = dto.ProductId;

            var getUserPoint = await _unitOfWork.pointRepository.GetPointsByUserId(dto.UserId);

            getUserPoint.Amount += 100;
            getUserPoint.UpdatedAt = DateTime.UtcNow.AddHours(7);
            _unitOfWork.pointRepository.Update(getUserPoint);

            var pointTransaction = new PointTransaction()
            {
                Point_Id = getUserPoint.Id,
                Amount = 100,
                Status = PointTransactionEnum.Earned,
                Description = "Bạn nhận được 100 xu từ việc đánh giá sản phẩm.",
            };
            await _unitOfWork.ratingRepository.AddAsync(rating);
            await _unitOfWork.pointTransactionRepository.AddAsync(pointTransaction);

            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Đánh giá thành công.",
                Data = rating
            };
        }
    }
}
