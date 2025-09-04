using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.Farmland;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class FarmlandService : IFarmlandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserQuestService _userQuestService;
        private readonly IMapper _mapper;

        public FarmlandService(IUnitOfWork unitOfWork, IMapper mapper, IUserQuestService userQuestService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userQuestService = userQuestService;
        }

        public async Task<Result<List<ViewFarmlandDTO>>> GetFarmlandsByUserIdAsync(Guid userId)
        {
            var getUser = await _unitOfWork.userRepository.GetByIdAsync(userId);
            if (getUser == null)
            {
                return new Result<List<ViewFarmlandDTO>>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ViewFarmlandDTO>>(await _unitOfWork.farmlandRepository.GetByUserIdAsync(userId));
            return new Result<List<ViewFarmlandDTO>>
            {
               Error = 0,
               Message = "Lấy danh sách ô đất của ngươi dùng thành công",
               Data = result
            };
        }

        public async Task<Result<object>> PlowAsync(PlowCropDTO plowCropDTO)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(plowCropDTO.UserId);
            var getFarmLandCrop = await _unitOfWork.farmlandRepository.GetFarmLandWithUserIdAndTileIdAsync(plowCropDTO.UserId, plowCropDTO.TileId);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            }

            if (getFarmLandCrop == null)
            {
                var newFarmLand = new FarmLand
                {
                    UserId = plowCropDTO.UserId,
                    TileId = plowCropDTO.TileId,
                    Watered = false,
                    Status = FarmLandStatus.Plowed,
                };
                await _unitOfWork.farmlandRepository.AddAsync(newFarmLand);
                await _unitOfWork.SaveChangeAsync();
                return new Result<object>
                {
                    Error = 0,
                    Message = "Xới đất thành công",
                    Data = null
                };
            }
            ;
            if (getFarmLandCrop.Status != FarmLandStatus.Empty)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất vẫn đang có cây trồng hoặc đã được xới",
                    Data = null
                };
            }
            getFarmLandCrop.Status = FarmLandStatus.Plowed;
            _unitOfWork.farmlandRepository.Update(getFarmLandCrop);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Xới đất thành công",
                Data = null
            };
        }

        public async Task<Result<object>> PlantCropAsync(PlantCropDTO plant)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(plant.UserId);
            var item = await _unitOfWork.itemRepository.GetByIdAsync(plant.ItemId);
            var getFarmLand = await _unitOfWork.farmlandRepository.GetFarmLandWithUserIdAndTileIdAsync(plant.UserId, plant.TileId);
            var getFarmLandCrop = await _unitOfWork.farmlandCropRepository.GetFarmLandCropWithUserIdAndTileIdAsync(plant.UserId, plant.TileId);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại",
                    Data = null
                };
            }

            if (getFarmLand == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = null
                };
            }

            if (item == null || item.ItemType != ItemTypeEnum.Seed)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Hạt giống không tồn tại hoặc không hợp lệ",
                    Data = null
                };
            }

            if (getFarmLand.Status == FarmLandStatus.Empty)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Đất chưa được xới nên không thể trồng cây",
                    Data = null
                };
            }

            if (getFarmLand.Status == FarmLandStatus.Planted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Đất đang được trồng cây",
                    Data = null
                };
            }

            if(getFarmLandCrop == null)
            {
                var farmlandCrop = new FarmlandCrop
                {
                    FarmlandId = getFarmLand.Id,
                    UserId = plant.UserId,
                    TileId = plant.TileId,
                    SeedId = plant.ItemId,
                    Stage = 0,
                    NeedsWater = false,
                    NextWaterDueAtUtc = plant.NextWaterDueAtUtc,
                    PlantedAtUtc = DateTime.UtcNow.AddHours(7),
                    IsActive = true
                };
                await _unitOfWork.farmlandCropRepository.AddAsync(farmlandCrop);
            }
            else
            {
                if(getFarmLandCrop.IsActive)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Ô đất vẫn đang có cây trồng",
                        Data = null
                    };
                }
                else
                {
                    getFarmLandCrop.SeedId = plant.ItemId;
                    getFarmLandCrop.Stage = 0;
                    getFarmLandCrop.NeedsWater = false;
                    getFarmLandCrop.NextWaterDueAtUtc = plant.NextWaterDueAtUtc;
                    getFarmLandCrop.PlantedAtUtc = DateTime.UtcNow.AddHours(7);
                    getFarmLandCrop.IsActive = true;
                    _unitOfWork.farmlandCropRepository.Update(getFarmLandCrop);
                }
            }

            getFarmLand.Status = FarmLandStatus.Planted;
            getFarmLand.PlantedAt = DateTime.UtcNow.AddHours(7);
            getFarmLand.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.farmlandRepository.Update(getFarmLand);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Trồng cây thành công",
                Data = null
            };
        }

        public async Task<Result<object>> HarvestAsync(HavestCropDTO havest)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(havest.UserId);
            var getFarmLand = await _unitOfWork.farmlandRepository.GetFarmLandWithUserIdAndTileIdAsync(havest.UserId, havest.TileId);
            var getFarmLandCrop = await _unitOfWork.farmlandCropRepository.GetFarmLandCropWithUserIdAndTileIdAsync(havest.UserId, havest.TileId);
            if (getFarmLand == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = null
                };
            };

            if (getFarmLand.Status != FarmLandStatus.Planted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Đất chưa được trồng nên không thể thu hoạch",
                    Data = null
                };
            }

            if(getFarmLandCrop == null || !getFarmLandCrop.IsActive)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất chưa có cây trồng để thu hoạch",
                    Data = null
                };
            }

            getFarmLand.Status =FarmLandStatus.Empty;
            getFarmLand.PlantedAt = null;   
            getFarmLand.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.farmlandRepository.Update(getFarmLand);

            getFarmLandCrop.Stage = 0;
            getFarmLandCrop.NeedsWater = false;
            getFarmLandCrop.NextWaterDueAtUtc = null;
            getFarmLandCrop.PlantedAtUtc = null;
            getFarmLandCrop.StageEndsAtUtc = null;
            getFarmLandCrop.HarvestedAtUtc = DateTime.UtcNow.AddHours(7);
            getFarmLandCrop.IsActive = false;
            _unitOfWork.farmlandCropRepository.Update(getFarmLandCrop);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Thu hoạch cây trồng thành công",
                Data = null
            };
        }

        public async Task<Result<object>> WaterAsync(WateredCropDTO water)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(water.UserId);
            var getFarmLand = await _unitOfWork.farmlandRepository.GetFarmLandWithUserIdAndTileIdAsync(water.UserId, water.TileId);
            var getFarmLandCrop = await _unitOfWork.farmlandCropRepository.GetFarmLandCropWithUserIdAndTileIdAsync(water.UserId, water.TileId);
            if (getFarmLand == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất không tồn tại",
                    Data = null
                };
            }
            ;

            if (getFarmLand.Status != FarmLandStatus.Planted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Đất chưa được trồng nên không thể tưới nước",
                    Data = null
                };
            }

            if (getFarmLandCrop == null || !getFarmLandCrop.IsActive)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Ô đất chưa có cây trồng để thu hoạch",
                    Data = null
                };
            }
            getFarmLandCrop.Stage++;
            getFarmLandCrop.NeedsWater = false;
            getFarmLandCrop.NextWaterDueAtUtc = water.NextWaterDueAtUtc;

            _unitOfWork.farmlandCropRepository.Update(getFarmLandCrop);
            await _unitOfWork.SaveChangeAsync();
            await _userQuestService.UpdateProgressAsync(user.Id, QuestType.WaterPlant, 1);
            return new Result<object>
            {
                Error = 0,
                Message = "Tưới nước thành công",
                Data = null
            };
        }
    }
}
