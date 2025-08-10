using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Inventory;
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
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewInventoryDTO>>> GetByUserIdAsync(Guid userId)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (checkUser == null)
            {
                return new Result<List<ViewInventoryDTO>>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewInventoryDTO>>(await _unitOfWork.inventoryRepository.GetByUserIdAsync(userId));
            return new Result<List<ViewInventoryDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách đồ vật trong kho đồ thành công.",
                Data = result,
            };
        }

        public async Task<Result<ViewInventoryDTO>> GetInventoryById(Guid inventoryId)
        {
            var inventory = _mapper.Map<ViewInventoryDTO>(await _unitOfWork.inventoryRepository.GetByIdAsync(inventoryId));
            if (inventory == null)
            {
                return new Result<ViewInventoryDTO>()
                {
                    Error = 1,
                    Message = "Kho đồ không tồn tại.",
                    Data = null
                };
            }
            return new Result<ViewInventoryDTO>()
            {
                Error = 0,
                Message = "Lấy kho đồ thành công.",
                Data = inventory
            };
        }

        public async Task<Result<object>> AddToInventoryAsync(AddToInventoryDTO addToInventoryDTO)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(addToInventoryDTO.UserId);
            if (checkUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<Inventory>(addToInventoryDTO);
            await _unitOfWork.inventoryRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Thêm đồ vật vào kho đồ thành công.",
                Data = addToInventoryDTO
            };
        }

        public async Task<Result<object>> UpdateInventoryAsync(UpdateInventoryDTO updateInventoryDTO)
        {
            var checkInventory = await _unitOfWork.inventoryRepository.GetByIdAsync(updateInventoryDTO.Id);
            if (checkInventory == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kho đồ không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map(updateInventoryDTO, checkInventory);
            _unitOfWork.inventoryRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật kho đồ thành công.",
                Data = result
            };
        }

        public async Task<Result<object>> DeleteInventoryAsync(Guid inventoryId)
        {
            var checkInventory = await _unitOfWork.inventoryRepository.GetByIdAsync(inventoryId);
            if (checkInventory == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kho đồ không tồn tại.",
                    Data = null
                };
            }

            _unitOfWork.inventoryRepository.Remove(checkInventory);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Xoá kho đồ thành công.",
                Data = null
            };
        }
    }
}
