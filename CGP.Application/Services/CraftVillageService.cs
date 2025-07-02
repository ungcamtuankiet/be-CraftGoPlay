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
            if (craftVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Làng thủ công không thể rỗng."
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Village_Name))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Tên làng là bắt buộc."
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Location))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Vị trí của làng là bắt buộc."
                };
            }

            if (craftVillage.EstablishedDate > DateTime.UtcNow)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Ngày thành lập không thể là ngày trong tương lai."
                };
            }

            var existingVillage = await _unitOfWork.craftVillageRepository.GetByNameAsync(craftVillage.Village_Name);
            if (existingVillage != null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Làng nghề có tên {craftVillage.Village_Name} đã tồn tại."
                };
            }

            try
            {
                var newVillage = _mapper.Map<CraftVillage>(craftVillage);
                newVillage.Id = Guid.NewGuid(); 

                await _unitOfWork.craftVillageRepository.AddAsync(newVillage);
                await _unitOfWork.SaveChangeAsync();

                return new Result<CraftVillage>
                {
                    Error = 0,
                    Message = "Làng nghề được tạo thành công.",
                    Data = newVillage
                };
            }
            catch (Exception ex)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Không thể tạo ra làng nghề: {ex.Message}"
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
                Message = "Lấy danh sách làng nghề thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewCraftVillageDTO>> GetCraftVillageByIdAsync(Guid id)
        {
            var craftVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            var result = _mapper.Map<ViewCraftVillageDTO>(craftVillage);
            if (craftVillage == null)
            {
                throw new KeyNotFoundException($"Làng nghề với mã {id} không được tìm thấy.");
            }
            return new Result<ViewCraftVillageDTO>
            {
                Error = 0,
                Message = "Lấy thông tin làng nghề thành công.",
                Data = result
            };
        }

        public async Task<Result<CraftVillage>> UpdateCraftVillage(Guid id, UpdateCraftVillageDTO craftVillage)
        {
            if (id == Guid.Empty)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Mã làng nghề không hợp lệ."
                };
            }

            if (craftVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Vui lòng điền đầy đủ thông tin"
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Village_Name))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Tên làng nghề là bắt buộc."
                };
            }

            if (string.IsNullOrWhiteSpace(craftVillage.Location))
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Vị trí làng nghề là bắt buộc."
                };
            }

            if (craftVillage.EstablishedDate > DateTime.UtcNow)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Ngày thành lập không thể là ngày trong tương lai."
                };
            }

            var existingVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            if (existingVillage == null)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Mã làng nghề: {id} không tìm thấy."
                };
            }

            var duplicateVillage = await _unitOfWork.craftVillageRepository.GetByNameAsync(craftVillage.Village_Name);
            if (duplicateVillage != null && duplicateVillage.Id != id)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Làng nghề với tên: '{craftVillage.Village_Name}' đã tồn tại."
                };
            }

            try
            {
                _mapper.Map(craftVillage, existingVillage);

                await _unitOfWork.craftVillageRepository.UpdateCraftVillage(existingVillage);
                await _unitOfWork.SaveChangeAsync();

                return new Result<CraftVillage>
                {
                    Error = 0,
                    Message = "Cập nhật thông tin làng nghề thành công.",
                    Data = existingVillage
                };
            }
            catch (Exception ex)
            {
                return new Result<CraftVillage>
                {
                    Error = 1,
                    Message = $"Cập nhật làng nghề thất bại: {ex.Message}"
                };
            }
        }
        public async Task<Result<object>> DeleteCraftVillage(Guid id)
        {
            var existingVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(id);
            if (existingVillage == null)
            {
                throw new KeyNotFoundException($"Mã làng nghề với: {id} không tìm thấy.");
            }
            _unitOfWork.craftVillageRepository.Remove(existingVillage);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Xóa làng nghề thành công.",
                Data = null
            };
        }
    }
}