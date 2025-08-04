using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Point;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PointService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Implement methods for point service here
        
        public async Task<Result<ViewPointDTO>> GetPointsByUserId(Guid userId)
        {
            var points = await _unitOfWork.pointRepository.GetPointsByUserId(userId);
            var result = _mapper.Map<ViewPointDTO>(points);
            return new Result<ViewPointDTO>
            {
                Error = 0,
                Message = "Lấy thông tin điểm thành công.",
                Data = result
            };
        }
    }
}
