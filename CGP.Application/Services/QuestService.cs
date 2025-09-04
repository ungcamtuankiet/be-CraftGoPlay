using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Quest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class QuestService : IQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<object>> CreateQuest(CreateQuestDTO createQuestDTO)
        {
            var result = _mapper.Map<Quest>(createQuestDTO);
            await _unitOfWork.questRepository.AddAsync(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Tạo nhiệm vụ thành công",
                Data = null
            };
        }

        public async Task<Result<List<ViewQuestDTO>>> GetAllQuests()
        {
            var result = _mapper.Map<List<ViewQuestDTO>>(await _unitOfWork.questRepository.GetAllAsync());  
            return new Result<List<ViewQuestDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách nhiệm vụ thành công",
                Data = result
            };
        }

        public async Task<Result<ViewQuestDTO>> GetQuestById(Guid id)
        {
            var result =_mapper.Map<ViewQuestDTO>(_unitOfWork.questRepository.GetByIdAsync(id));
            if(result == null)
            {
                return new Result<ViewQuestDTO>()
                {
                    Error = 1,
                    Message = "Nhiệm vụ không tồn tại",
                    Data = null
                };
            }
            return new Result<ViewQuestDTO>()
            {
                Error = 0,
                Message = "Lấy nhiệm vụ thành công",
                Data = result
            };
        }
    }
}
