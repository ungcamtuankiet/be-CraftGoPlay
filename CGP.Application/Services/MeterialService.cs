using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Meterial;
using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class MeterialService : IMeterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MeterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewMeterialDTO>>> GetMeterialsAsync()
        {

            var result = _mapper.Map<List<ViewMeterialDTO>>(await _unitOfWork.meterialRepository.GetMeterialsAsync());
            return new Result<List<ViewMeterialDTO>>
            {
                Error = 0,
                Message = "Get successfully",
                Data = result
            };
        }

        public async Task<Result<object>> CreateMeterial(MeterialCreateDTO request)
        {
            var meterial = _mapper.Map<Meterial>(request);

            await _unitOfWork.meterialRepository.CreateMeterialAsync(meterial);
            return new Result<object>
            {
                Error = 0,
                Message = "Meterial created successfully",
                Data = _mapper.Map<ViewMeterialDTO>(meterial)
            };
        }

        public async Task<Result<object>> UpdateMeterial(MeterialUpdateDTO request)
        {
            var getMeterial = await _unitOfWork.meterialRepository.GetMeterialByIdAsync(request.Id  );
            if (getMeterial == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Meterial not found",
                    Data = null
                };
            }
            _mapper.Map(request, getMeterial);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Meterial created successfully",
                Data = _mapper.Map<ViewMeterialDTO>(request)
            };
        }

        public async Task<Result<object>> DeleteMeterial(Guid id)
        {
            var getMeterial = await _unitOfWork.meterialRepository.GetMeterialByIdAsync(id);
            if(getMeterial == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Meterial not found",
                    Data = null
                };
            }
            await _unitOfWork.meterialRepository.DeleteMeterialAsync(getMeterial);
            return new Result<object>
            {
                Error = 0,
                Message = "Meterial deleted successfully",
                Data = null
            };
        }
    }
}
