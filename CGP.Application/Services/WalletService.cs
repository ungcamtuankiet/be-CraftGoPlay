using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.Wallet;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
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
    }
}
