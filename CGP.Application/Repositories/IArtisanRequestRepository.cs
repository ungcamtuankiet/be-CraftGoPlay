using CGP.Contract.DTO.ArtisanRequest;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IArtisanRequestRepository : IGenericRepository<ArtisanRequest>
    {
        public Task<List<ArtisanRequest>> GetAllArtisanRequests();
        public Task<List<ArtisanRequest>> GetRequestByStatus(int pageIndex, int pageSize, RequestArtisanStatus status);
        public Task<ArtisanRequest> GetArtisanRequestById(Guid id);
        public Task SendNewRequest(ArtisanRequest artisanRequest);
        public Task<ArtisanRequest?> GetPendingRequestByUserId(Guid userId);
        public Task CancelRequestByArtisan(ArtisanRequest artisanRequest);
        public Task AcceptRequest(ArtisanRequest artisanRequest);
        public Task RejectRequest(ArtisanRequest artisanRequest);
        public Task<ArtisanRequest?> GetLatestRequestByUserId(Guid userId);
        public Task<ArtisanRequest?> GetRequestByIdAndUserId(Guid requestId, Guid userId);
        public Task<ArtisanRequest> CheckPhoneNo(string phoneNo);
    }
}
