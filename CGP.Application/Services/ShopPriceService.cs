using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Item;
using CGP.Contract.DTO.ShopPrice;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public async Task<Result<List<ViewItemsSeillDTO>>> GetItemsSell()
        {
            var result = _mapper.Map<List<ViewItemsSeillDTO>>(await _unitOfWork.shopPriceRepository.GetAllShopPrice());
            return new Result<List<ViewItemsSeillDTO>>()
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

        public async Task<Result<SellResponse>> SellItem(SellRequest request)
        {
            var getItemInInventory = await _unitOfWork.inventoryRepository.GetItemInInventory(request.UserId, request.ItemId);
            var getItemInShop = await _unitOfWork.shopPriceRepository.GetItemInShop(request.ItemId);
            var getUserPoint = await _unitOfWork.pointRepository.GetPointsByUserId(request.UserId);
            var getItem = await _unitOfWork.itemRepository.GetByIdAsync(request.ItemId);
            var getUser = await _unitOfWork.userRepository.GetByIdAsync(request.UserId);

            if(getUser == null)
            {
                return new Result<SellResponse>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại"
                };
            }

            if (getItemInInventory == null)
            {
                return new Result<SellResponse>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại"
                };
            }

            if(getItemInInventory.Quantity < request.Quantity)
            {
                return new Result<SellResponse>()
                {
                    Error = 1,
                    Message = "Số lượng vật phẩm không đủ để bán"
                };
            }

            if(getItemInShop == null)
            {
                return new Result<SellResponse>()
                {
                    Error = 1,
                    Message = "Vật phẩm không tồn tại trong cửa hàng"
                };
            }

            getUserPoint.Amount += (request.Quantity * getItemInShop.Price);
            _unitOfWork.pointRepository.Update(getUserPoint);
            var newPointTransaction = new PointTransaction()
            {
                Point_Id = getUserPoint.Id,
                Amount = request.Quantity * getItemInShop.Price,
                Status = PointTransactionEnum.Earned,
                Description = $"Bán {request.Quantity} {getItem.NameItem}",
                CreationDate = DateTime.UtcNow.AddHours(7)
            };
            await _unitOfWork.pointTransactionRepository.AddAsync(newPointTransaction);
            getItemInInventory.Quantity -= request.Quantity;
            _unitOfWork.inventoryRepository.Update(getItemInInventory);
            await _unitOfWork.SaveChangeAsync();
            return new Result<SellResponse>()
            {
                Error = 0,
                Message = "Bán vật phẩm thành công",
                Data = new SellResponse()
                {
                    ReceivedCoins = request.Quantity *  getItemInShop.Price,
                    ItemId = request.ItemId,
                    QuantitySold = request.Quantity,
                    UnitPrice = getItemInShop.Price,
                    ToalPrice = request.Quantity * getItemInShop.Price,
                    NewBalance = getUserPoint.Amount
                }
            };
        }
    }
}
