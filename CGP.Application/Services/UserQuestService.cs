using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserQuest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class UserQuestService : IUserQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserQuestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewUserQuestDTO>>> GetUserQuestsAsync(Guid userId)
        {
            var result = _mapper.Map<List<ViewUserQuestDTO>>(await _unitOfWork.userQuestRepository.GetUserQuests(userId));
            return new Result<List<ViewUserQuestDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách nhiệm vụ thành công.",
                Data = result
            };
        }

        public async Task EnsureDailyQuestAsync(Guid userId)
        {
            var checkExistQuest = await _unitOfWork.userQuestRepository.CheckExistQuest(userId);
            if (checkExistQuest != null) return;

            var dailyQuests = await _unitOfWork.questRepository.GetDailyQuests();
            if (dailyQuests == null) return;

            foreach (var quest in dailyQuests)
            {
                var existQuest = await _unitOfWork.userQuestRepository
                    .GetUserQuestAsync(userId, quest.Id);

                if (existQuest != null) continue; 

                var userQuest = new UserQuest
                {
                    UserId = userId,
                    QuestId = quest.Id,
                    RewardClaimed = false,
                    CreationDate = DateTime.UtcNow.AddHours(7),
                    CompletedAt = null
                };
                if (quest.QuestType == QuestType.DailyLogin)
                {
                    userQuest.Progress = 1;
                    userQuest.Status = QuestStatus.Completed;
                    userQuest.CompletedAt = DateTime.UtcNow.AddHours(7);
                }
                else
                {
                    userQuest.Progress = 0;
                    userQuest.Status = QuestStatus.Pending;
                    userQuest.CompletedAt = null;
                }

                await _unitOfWork.userQuestRepository.AddAsync(userQuest);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Result<object>> ClaimDailyRewardAsync(Guid userId, Guid userQusetId)
        {
            var userQuest = await _unitOfWork.userQuestRepository.GetUserQuestAsync(userId, userQusetId);
            var getReward = await _unitOfWork.questRepository.GetByIdAsync(userQuest.QuestId);
            var getItemInventory = await _unitOfWork.inventoryRepository.GetItemInInventory(userId, getReward.Reward);
            if (userQuest == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Nhiệm vụ không tồn tại.",
                    Data = null
                };
            }

            if (!userQuest.Status.Equals(QuestStatus.Completed))
                return new Result<object> { Error = 1, Message = "Quest chưa hoàn thành" };

            if (userQuest.RewardClaimed)
                return new Result<object> { Error = 1, Message = "Quà đã được nhận" };

            var packageItems = await _unitOfWork.inventoryRepository.GetItemsInInventoryTypeAsync(userId);
            if(getItemInventory != null)
            {
                getItemInventory.Quantity += userQuest.Quest.AmountReward;
                _unitOfWork.inventoryRepository.Update(getItemInventory);
            }
            else
            {
                var usedSlots = packageItems.Select(x => x.SlotIndex).ToHashSet();

                int freeSlot = Enumerable.Range(0, 27).FirstOrDefault(slot => !usedSlots.Contains(slot));

                if (freeSlot == 0 && usedSlots.Contains(0))
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Túi đã đầy, không thể nhận thưởng.",
                        Data = null
                    };
                }



                await _unitOfWork.inventoryRepository.AddAsync(new Inventory
                {
                    UserId = userId,
                    ItemId = getReward.Reward,
                    Quantity = userQuest.Quest.AmountReward,
                    InventoryType = "Backpack",
                    SlotIndex = freeSlot
                });
            }
            userQuest.RewardClaimed = true;
            _unitOfWork.userQuestRepository.Update(userQuest);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Nhận phần thưởng thành công.",
                Data = null
            };
        }

        public async Task UpdateProgressAsync(Guid userId, QuestType questType, int amount)
        {
            var userQuest = await _unitOfWork.userQuestRepository.GetByUserAndQuestTypeAsync(userId, questType);
            if (userQuest == null) return;
            userQuest.Progress += amount;
            if(questType == QuestType.WaterPlant)
            {
                if (userQuest.Progress >= 5)
                {
                    userQuest.Progress = 5;
                    userQuest.Status = QuestStatus.Completed;
                    userQuest.CompletedAt = DateTime.UtcNow.AddHours(7);
                }
            }
            else
            {
                if (userQuest.Progress >= 1)
                {
                    userQuest.Progress = 1;
                    userQuest.Status = QuestStatus.Completed;
                    userQuest.CompletedAt = DateTime.UtcNow.AddHours(7);
                }
            }
            _unitOfWork.userQuestRepository.Update(userQuest);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
