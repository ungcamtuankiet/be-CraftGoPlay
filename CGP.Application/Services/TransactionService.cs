using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Transaction;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ViewTransactionDTO>>> GetAllTransactionsAsync()
        {
            var result = _mapper.Map<List<ViewTransactionDTO>>(await _unitOfWork.transactionRepository.GetTransactions());
            return new Result<List<ViewTransactionDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách giao dịch thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewTransactionDTO>> GetTransactionByIdAsync(Guid transactionId)
        {
            var result = _mapper.Map<ViewTransactionDTO>(await _unitOfWork.transactionRepository.GetByIdAsync(transactionId));
            if(result == null)
            {
                return new Result<ViewTransactionDTO>()
                {
                    Error = 1,
                    Message = "Giao dịch không tồn tại.",
                    Data = null
                };
            }

            return new Result<ViewTransactionDTO>()
            {
                Error = 0,
                Message = "Lấy thông tin giao dịch thành công.",
                Data = result
            };
        }

        public async Task<Result<List<ViewTransactionDTO>>> GetAllTransactionsByUserIdAsync(Guid userId)
        {
            var checkUser = await _unitOfWork.userRepository.GetUserById(userId);
            if (checkUser == null)
            {
                return new Result<List<ViewTransactionDTO>>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewTransactionDTO>>(await _unitOfWork.transactionRepository.GetTransactionsByUserIdAsync(userId));
            return new Result<List<ViewTransactionDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách giao dịch của người dùng thành công.",
                Data = result
            };
        }
    }
}
