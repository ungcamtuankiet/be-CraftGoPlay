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
    }
}
