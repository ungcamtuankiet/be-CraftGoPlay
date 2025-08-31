using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.Farmland;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class FarmlandService : IFarmlandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FarmlandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewFarmlandDTO>>> GetFarmlandsByUserIdAsync(Guid userId)
        {
            var getUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (getUser == null)
            {
                return new Result<List<ViewFarmlandDTO>>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ViewFarmlandDTO>>(await _unitOfWork.farmlandRepository.GetByUserIdAsync(userId));
            return new Result<List<ViewFarmlandDTO>>
            {
               Error = 0,
               Message = "Lấy danh sách ô đất của ngươi dùng thành công",
               Data = result
            };
        }

        public async Task<Result<bool>> DigAsync(Guid plotId)
        {
            var plot = await _unitOfWork.farmlandRepository.GetFarmlandByIdAsync(plotId);
            if (plot == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = false
                };
            }

            _unitOfWork.farmlandRepository.Update(plot);
            await _unitOfWork.SaveChangeAsync();
            return new Result<bool>
            {
                Error = 0,
                Message = "Đào đất thành công",
                Data = true
            };
        }

        public async Task<Result<bool>> PlantAsync(Guid plotId)
        {
            var plot = await _unitOfWork.farmlandRepository.GetFarmlandByIdAsync(plotId);
            var activeCrop = await _unitOfWork.farmlandCropRepository.GetActiveCropAsync(plotId);
            if (plot == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = false
                };
            }

            if(activeCrop != null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Ô đất đang có cây trồng chưa thu hoạch",
                    Data = false
                };
            }
            plot.PlantedAt = DateTime.UtcNow;

            _unitOfWork.farmlandRepository.Update(plot);
            await _unitOfWork.SaveChangeAsync();
            return new Result<bool>
            {
                Error = 0,
                Message = "Trồng cây thành công",
                Data = true
            };
        }

        public async Task<Result<bool>> ResetAsync(Guid plotId)
        {
            var plot = await _unitOfWork.farmlandRepository.GetFarmlandByIdAsync(plotId);
            if (plot == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = false
                };
            }

            _unitOfWork.farmlandRepository.Update(plot);
            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = 0,
                Message = "Đặt lại ô đất thành công",
                Data = true
            };
        }

        public Task<Result<bool>> WaterAsync(Guid plotId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> HarvestAsync(Guid plotId)
        {
            throw new NotImplementedException();
        }
    }
}
