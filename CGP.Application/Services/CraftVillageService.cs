using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.CraftVillage;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class CraftVillageService : ICraftVillageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CraftVillageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task CreateNewCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<ViewCraftVillageDTO>>> GetAllCraftVillagesAsync()
        {
            var result = _mapper.Map<List<ViewCraftVillageDTO>>(await _unitOfWork.craftVillageRepository.GetAllCraftVillagesAsync());
            return new Result<List<ViewCraftVillageDTO>>
            {
                Error = 0,
                Message = "Get all craft village successfully.",
                Data = result
            };
        }

        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }
    }
}
