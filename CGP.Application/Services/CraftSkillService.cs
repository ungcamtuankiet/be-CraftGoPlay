using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.CraftSkill;
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
    public class CraftSkillService : ICraftSkillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CraftSkillService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> CreateNewCraftSkill(CreateCraftSkillDTO createCraftSkillDTO)
        {
            var craftSkill = _mapper.Map<CraftSkill>(createCraftSkillDTO);
            await _unitOfWork.craftSkillRepository.AddAsync(craftSkill);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Tạo kỹ năng thành công",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteCraftSkill(Guid id)
        {
            var craftSkill = await _unitOfWork.craftSkillRepository.GetCraftSkillByIdAsync(id);
            if (craftSkill == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kỹ năng không tồn tại",
                    Data = null
                };
            }
            _unitOfWork.craftSkillRepository.Remove(craftSkill);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Xóa kỹ năng thành công",
                Data = null
            };
        }

        public async Task<Result<IEnumerable<ViewCraftSkillDTO>>> GetAllCraftSkillsAsync()
        {
            var result = _mapper.Map<IEnumerable<ViewCraftSkillDTO>>(await _unitOfWork.craftSkillRepository.GetAllAsync());
            return new Result<IEnumerable<ViewCraftSkillDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách kỹ năng thành công",
                Data = result
            };
        }

        public async Task<Result<ViewCraftSkillDTO>> GetCraftSkillByIdAsync(Guid id)
        {
            var craftSkill = await _unitOfWork.craftSkillRepository.GetCraftSkillByIdAsync(id);
            if (craftSkill == null)
            {
                return new Result<ViewCraftSkillDTO>()
                {
                    Error = 1,
                    Message = "Kỹ năng không tồn tại",
                    Data = null
                };
            }
            var result = _mapper.Map<ViewCraftSkillDTO>(craftSkill);
            return new Result<ViewCraftSkillDTO>()
            {
                Error = 0,
                Message = "Lấy kỹ năng thành công",
                Data = result
            };
        }

        public async Task<Result<object>> UpdateNewCraftSkill(UpdateCraftSkillDTO updateCraftSkillDTO)
        {
            var craftSkill = await _unitOfWork.craftSkillRepository.GetCraftSkillByIdAsync(updateCraftSkillDTO.Id);
            if (craftSkill == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kỹ năng không tồn tại",
                    Data = null
                };
            }
            craftSkill.Name = updateCraftSkillDTO.Name;
            _unitOfWork.craftSkillRepository.Update(craftSkill);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật kỹ năng thành công",
                Data = null
            };
        }
    }
}
