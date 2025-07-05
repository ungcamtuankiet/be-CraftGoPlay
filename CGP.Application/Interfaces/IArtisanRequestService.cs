using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IArtisanRequestService
    {
        public Task<Result<List<ViewRequestDTO>>> GetAllRequestsAsync();
        public Task<Result<ViewRequestDTO>> GetRequestByIdAsync(Guid id);
        public Task<Result<List<ViewRequestDTO>>> GetRequestByStatus(int pageIndex, int pageSize, RequestArtisanStatus status);
        public Task<Result<ViewRequestDTO>> SendRequestAsync(SendRequestDTO requestDto);
        public Task<ArtisanRequest?> GetPendingRequestByUserId(Guid userId);
        public Task<Result<object>> CancelRequestByArtisan(Guid id);
        public Task<Result<object>> ApprovedRequest(Guid id);
        public Task<Result<object>> RejectedRequest(RejectRequestDTO reject);
        public Task<Result<ViewRequestDTO>> GetLatestRequestByUserId(Guid userId);
        public Task<Result<object>> ResendRequest(Guid userId, Guid requestId);
    }
}
