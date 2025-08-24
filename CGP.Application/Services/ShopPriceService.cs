using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Item;
using CGP.Contract.DTO.ShopPrice;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class ShopPriceService : IShopPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopPriceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> GetAllItemShopPrice()
        {
            var result = _mapper.Map<List<ViewItemShopPriceDTO>>(await _unitOfWork.shopPriceRepository.GetAllShopPrice());
            return new Result<object>()
            {
                Error = 0,
                Message = "Lấy danh sách vật phẩm được treo bán thành công",
                Data = result
            };
        }

        public async Task<Result<object>> GetItemShopPrice(Guid shopId)
        {
            var result = _mapper.Map<ViewItemShopPriceDTO>(await _unitOfWork.shopPriceRepository.GetShopPriceById(shopId));
            return new Result<object>()
            {
                Error = 0,
                Message = "Lấy vật phẩm được treo bán thành công",
                Data = result
            };
        }

        public async Task<Result<object>> CreateShopPriceItem(CreateItemShopPriceDTO dto)
        {
            var checkItem = await _unitOfWork.itemRepository.GetByIdAsync(dto.ItemId);
            var checkExistItemInShop = await _unitOfWork.shopPriceRepository.GetItemInShop(dto.ItemId);
            if(checkItem == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại"
                };
            }

            if(checkExistItemInShop != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vật phẩm đã tồn tại trong shop"
                };
            }


            if(dto.Price <= 0)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Giá bán phải lớn hơn 0",
                    Data = null
                };
            }

            var result = _mapper.Map<ShopPrice>(dto);
            await _unitOfWork.shopPriceRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Đưa vật phẩm lên cửa hàng để bán thành công",
                Data = dto
            };
        }

        public async Task<Result<object>> UpdateShopPriceItem(UpdateItemShopPriceDTO dto)
        {
            var getShopPrice = await _unitOfWork.shopPriceRepository.GetShopPriceById(dto.Id);
            if(getShopPrice == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại"
                };
            }   
            
            if (dto.Price <= 0)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Giá bán phải lớn hơn 0",
                    Data = null
                };
            }

            var result = _mapper.Map(dto, getShopPrice);
            _unitOfWork.shopPriceRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật vật phẩm lên cửa hàng để bán thành công",
                Data = null
            };
        }

        public async Task<Result<object>> RemoveShopPriceItem(Guid id)
        {
            var getShopPrice = await _unitOfWork.shopPriceRepository.GetShopPriceById(id);
            if (getShopPrice == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại"
                };
            }

            getShopPrice.IsDeleted = true;
            return new Result<object>()
            {
                Error = 0,
                Message = "Gỡ bỏ vật phẩm trên cửa hàng thành công"
            };
        }
    }
}
