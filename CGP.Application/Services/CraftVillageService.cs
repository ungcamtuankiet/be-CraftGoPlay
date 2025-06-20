using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.CraftVillage;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;

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

        public async Task<Result<CraftVillage>> CreateNewCraftVillage(CreateCraftVillageDTO craftVillage)
        {
            // Kiểm tra null ngay từ đầu
            if (craftVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Craft village cannot be null."
                };
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(craftVillage.Village_Name))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Village name is required."
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Location))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Location is required."
                };
            }

            if (craftVillage.EstablishedDate > DateTime.UtcNow)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Established date cannot be in the future."
                };
            }

            // Kiểm tra trùng lặp Village_Name (nếu cần)
            var existingVillage = await _unitOfWork.craftVillageRepository.GetByNameAsync(craftVillage.Village_Name);
            if (existingVillage != null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Craft village with name '{craftVillage.Village_Name}' already exists."
                };
            }

            try
            {
                // Ánh xạ DTO sang thực thể
                var newVillage = _mapper.Map<CraftVillage>(craftVillage);
                newVillage.Id = Guid.NewGuid(); // Gán Id mới

                await _unitOfWork.craftVillageRepository.AddAsync(newVillage);
                await _unitOfWork.SaveChangeAsync();

                return new Result<CraftVillage>
                {
                    Error = 0,
                    Message = "Craft village created successfully.",
                    Data = newVillage
                };
            }
            catch (Exception ex)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Failed to create craft village: {ex.Message}"
                };
            }
        }

        public async Task<Result<List<ViewCraftVillageDTO>>> GetAllCraftVillagesAsync()
        {
            var craftVillages = await _unitOfWork.craftVillageRepository.GetAllCraftVillagesAsync();
            var result = _mapper.Map<List<ViewCraftVillageDTO>>(craftVillages);

            return new Result<List<ViewCraftVillageDTO>>
            {
                Error = 0,
                Message = "Get all craft villages successfully.",
                Data = result
            };
        }

        public async Task<Result<ViewCraftVillageDTO>> GetCraftVillageByIdAsync(Guid id)
        {
            var craftVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            var result = _mapper.Map<ViewCraftVillageDTO>(craftVillage);
            if (craftVillage == null)
            {
                throw new KeyNotFoundException($"Craft village with ID {id} not found.");
            }
            return new Result<ViewCraftVillageDTO>
            {
                Error = 0,
                Message = "Get all craft villages successfully.",
                Data = result
            };
        }

        public async Task<Result<CraftVillage>> UpdateCraftVillage(Guid id, UpdateCraftVillageDTO craftVillage)
        {
            // Kiểm tra id hợp lệ
            if (id == Guid.Empty)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Invalid craft village ID."
                };
            }

            // Kiểm tra null
            if (craftVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Craft village cannot be null."
                };
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(craftVillage.Village_Name))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Village name is required."
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Location))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Location is required."
                };
            }

            if (craftVillage.EstablishedDate > DateTime.UtcNow)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Established date cannot be in the future."
                };
            }

            // Kiểm tra sự tồn tại của CraftVillage
            var existingVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            if (existingVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Craft village with ID {id} not found."
                };
            }

            // Kiểm tra trùng lặp Village_Name (ngoại trừ chính bản ghi đang cập nhật)
            var duplicateVillage = await _unitOfWork.craftVillageRepository.GetByNameAsync(craftVillage.Village_Name);
            if (duplicateVillage != null && duplicateVillage.Id != id)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Craft village with name '{craftVillage.Village_Name}' already exists."
                };
            }

            try
            {
                // Ánh xạ dữ liệu từ DTO vào thực thể hiện có
                _mapper.Map(craftVillage, existingVillage);

                await _unitOfWork.craftVillageRepository.UpdateCraftVillage(existingVillage);
                await _unitOfWork.SaveChangeAsync();

                return new Result<CraftVillage>
                {
                    Error = 0,
                    Message = "Craft village updated successfully.",
                    Data = existingVillage
                };
            }
            catch (Exception ex)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Failed to update craft village: {ex.Message}"
                };
            }
        }
        public async Task<Result<object>> DeleteCraftVillage(Guid id)
        {
            var existingVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            if (existingVillage == null)
            {
                throw new KeyNotFoundException($"Craft village with ID {id} not found.");
            }
            _unitOfWork.craftVillageRepository.Remove(existingVillage);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Craft village deleted successfully.",
                Data = null
            };
        }
    }
}