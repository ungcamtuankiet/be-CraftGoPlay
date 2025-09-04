using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Contract.DTO.Wallet;
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
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ViewWalletDTO>> GetWalletByUserId(Guid userId)
        {
            var result = _mapper.Map<ViewWalletDTO>(await _unitOfWork.walletRepository.GetWalletByUserIdAsync(userId));
            return new Result<ViewWalletDTO>
            {
                Error = 0,
                Message = "Lấy thông tin ví thành công.",
                Data = result
            };
        }
        public async Task<Result<ViewWalletDTO>> GetWalletByArtisanId(Guid artisanId)
        {
            var result = _mapper.Map<ViewWalletDTO>(await _unitOfWork.walletRepository.GetWalletByArtisanIdAsync(artisanId));
            return new Result<ViewWalletDTO>
            {
                Error = 0,
                Message = "Lấy thông tin ví thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewWalletDTO>> GetWalletSystem()
        {
            var result = _mapper.Map<ViewWalletDTO>(await _unitOfWork.walletRepository.GetWalletSystem());
            return new Result<ViewWalletDTO>
            {
                Error = 0,
                Message = "Lấy thông tin ví thành công.",
                Data = result
            };
        }

        public async Task CreatePendingTransactionAsync(Guid artisanId, decimal amount, int holdDays)
        {
            var wallet = await _unitOfWork.walletRepository.GetWalletByArtisanIdAsync(artisanId);

            if (wallet == null)
               throw new Exception("Ví nghệ nhân không tồn tại");

            var transaction = new WalletTransaction
            {
                Wallet_Id = wallet.Id,
                Amount = amount,
                Description = "Số tiền đang được chờ thanh toán cho sản phẩm hoàn thành",
                Type = WalletTransactionTypeEnum.Pending,
                UnlockDate = DateTime.UtcNow.AddHours(7).AddMinutes(holdDays)
            };

            wallet.PendingBalance += amount;

            await _unitOfWork.walletTransactionRepository.AddAsync(transaction);
            _unitOfWork.walletRepository.Update(wallet);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task ReleasePendingTransactionsAsync()
        {
            var now = DateTime.UtcNow.AddHours(7);

            // Lấy tất cả transaction đến hạn
            var pendingTransactions = await _unitOfWork.walletTransactionRepository
                .GetPendingTransactionsAsync(now);

            foreach (var transaction in pendingTransactions)
            {
                var wallet = transaction.Wallet;
                if (wallet == null) continue;

                wallet.PendingBalance -= transaction.Amount;
                wallet.AvailableBalance += transaction.Amount;

                // Update transaction status
                transaction.IsDeleted = true;

                // Nếu muốn log lịch sử, có thể tạo record mới
                await _unitOfWork.walletTransactionRepository.AddAsync(new WalletTransaction
                {
                    Wallet_Id = transaction.Wallet_Id,
                    Amount = transaction.Amount,
                    Description = "Số tiền đã được thanh toán cho sản phẩm",
                    Type = WalletTransactionTypeEnum.Release,
                    UnlockDate = now
                });

                _unitOfWork.walletRepository.Update(wallet);
                _unitOfWork.walletTransactionRepository.Update(transaction);
            }
            await _unitOfWork.SaveChangeAsync();
        }

    }
}
