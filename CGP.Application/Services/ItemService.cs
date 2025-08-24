using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Item;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> GetAllItemsAsync()
        {
            var result = _mapper.Map<List<ViewItemDTO>>(await _unitOfWork.itemRepository.GetAllAsync());
            return new Result<object>()
            {
                Error = 0,
                Message = "Lấy danh sách đồ vật thành công.",
                Data = result,
            };
        }

        public async Task<Result<object>> GetItemByIdAsync(Guid id)
        {
            var result = _mapper.Map<ViewItemDTO>(await _unitOfWork.itemRepository.GetByIdAsync(id));
            return new Result<object>()
            {
                Error = 0,
                Message = "Lấy đồ vật thành công.",
                Data = result,
            };
        }

        public async Task<Result<object>> CreateItem(CreateItemDTO createItemDTO)
        {
            var item = _mapper.Map<Item>(createItemDTO);
            await _unitOfWork.itemRepository.AddAsync(item);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Tạo đồ vật thành công.",
                Data = null,
            };
        }

        public async Task<Result<object>> UpdateItem(UpdateItemDTO updateItemDTO)
        {
            var getItem = await _unitOfWork.itemRepository.GetByIdAsync(updateItemDTO.Id);
            if (getItem == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Đồ vật không tồn tại.",
                    Data = null,
                };
            }
            var checkItemName = await _unitOfWork.itemRepository.GetItemByNameAsync(updateItemDTO.NameItem);
            if(checkItemName != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Tên đồ vật đã tồn tại.",
                    Data = null,
                };
            }

            var result = _mapper.Map(updateItemDTO, getItem);
            _unitOfWork.itemRepository.Update(result);
            _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật đồ vật thành công.",
                Data = null,
            };
        }

        public async Task<Result<object>> DeleteItem(Guid id)
        {
            var getItem = await _unitOfWork.itemRepository.GetByIdAsync(id);
            if (getItem == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Đồ vật không tồn tại.",
                    Data = null,
                };
            }
            getItem.IsDeleted = true;
            _unitOfWork.itemRepository.Update(getItem);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Xoá đồ vật thành công.",
                Data = null,
            };
        }
    }
}
