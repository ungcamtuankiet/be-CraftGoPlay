using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Crop;
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
    public class CropService : ICropService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CropService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> AddCropAsync(AddCropDTO request)
        {
            var result = _mapper.Map<Crop>(request);
            var checkName = await _unitOfWork.cropRepository.CheckName(result.Name);
            if(checkName != null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Tên cây trồng đã tồn tại.",
                    Data = null
                };
            }

            await _unitOfWork.cropRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Thêm cây trồng thành công.",
                Data = null
            };
        }

        public async Task<Result<ViewCropDTO>> GetCropByIdAsync(Guid cropId)
        {
            var result = _mapper.Map<ViewCropDTO>(
                await _unitOfWork.cropRepository.GetCropsByIdAsync(cropId)
            );
            if (result == null)
            {
                return new Result<ViewCropDTO>
                {
                    Error = 1,
                    Message = "Cây trồng không tồn tại.",
                    Data = null
                };
            }
            return new Result<ViewCropDTO>
            {
                Error = 0,
                Message = "Lấy thông tin cây trồng thành công.",
                Data = result
            };
        }

        public async Task<Result<object>> RemoveCropAsync(Guid cropId)
        {
            var crop = await _unitOfWork.cropRepository.GetCropsByIdAsync(cropId);
            if (crop == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Cây trồng không tồn tại.",
                    Data = null
                };
            }

            _unitOfWork.cropRepository.Remove(crop);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Xóa cây trồng thành công.",
                Data = null
            };
        }

        public async Task<Result<object>> UpdateCropAsync(UpdateCropDTO request)
        {
            var crop = await _unitOfWork.cropRepository.GetCropsByIdAsync(request.Id);
            if (crop == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Cây trồng không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map(request, crop);
            _unitOfWork.cropRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Cập nhật cây trồng thành công.",
                Data = null
            };
        }
    }
}
