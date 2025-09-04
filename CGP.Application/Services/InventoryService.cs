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

        public async Task<Result<object>> AddItemToInventoryAsync(AddItemToInventoryDTO addItemToInventoryDTO)
        {
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(addItemToInventoryDTO.UserId);
            var checkInventory = await _unitOfWork.inventoryRepository.CheckItemInSlotIndexAsync(addItemToInventoryDTO.UserId, addItemToInventoryDTO.SlotIndex, addItemToInventoryDTO.InventoryType);
            var checkItem = await _unitOfWork.itemRepository.GetByIdAsync((Guid)addItemToInventoryDTO.ItemId);
            if (checkUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            if(checkInventory != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vị trí đã tồn tại vật phẩm.",
                    Data = null
                };
            }

            if (checkUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            if(checkItem == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại"
                };
            }

            var result = _mapper.Map<Inventory>(addItemToInventoryDTO);
            await _unitOfWork.inventoryRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Thêm đồ vật vào kho đồ thành công.",
                Data = addItemToInventoryDTO
            };
        }

        public async Task<Result<object>> UpdateInventoryAsync(UpdateInventoryDTO updateInventoryDTO)
        {
            var getInventory = await _unitOfWork.inventoryRepository.GetByIdAsync(updateInventoryDTO.Id);
            var checkUser = await _unitOfWork.userRepository.GetByIdAsync(updateInventoryDTO.UserId);
            var checkInventory = await _unitOfWork.inventoryRepository.CheckItemInSlotIndexAsync(updateInventoryDTO.UserId, updateInventoryDTO.SlotIndex, updateInventoryDTO.InventoryType);
            var checkItem = await _unitOfWork.itemRepository.GetByIdAsync((Guid)updateInventoryDTO.ItemId);
            if (getInventory == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kho đồ không tồn tại.",
                    Data = null
                };
            }

            if (checkInventory != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Kho đồ đã tồn tại tại vị trí này.",
                    Data = null
                };
            }

            if (checkUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            _mapper.Map(updateInventoryDTO, getInventory);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật kho đồ thành công.",
                Data = null
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

        public async Task<Result<List<ViewItemsBackpackDTO>>> GetItemsInInventoryTypeAsync(Guid userId)
        {
            var result = _mapper.Map<List<ViewItemsBackpackDTO>>(await _unitOfWork.inventoryRepository.GetItemsInInventoryTypeAsync(userId));
            return new Result<List<ViewItemsBackpackDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách vật phẩm trong balo thành công.",
                Data = result
            };
        }
    }
}
