using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CGP.Application.Services
{
    public class ArtisanRequestService : IArtisanRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "artisan_request";

        public ArtisanRequestService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<List<ViewRequestDTO>>> GetAllRequestsAsync()
        {
            var result = _mapper.Map<List<ViewRequestDTO>>(await _unitOfWork.artisanRequestRepository.GetAllArtisanRequests());
            return new Result<List<ViewRequestDTO>>
            {
                Error = 0,
                Message = "Get All Request successfully.",
                Data = result,
            };
        }

        public async Task<Result<ViewRequestDTO>> GetRequestByIdAsync(Guid id)
        {
            var result = _mapper.Map<List<ViewRequestDTO>>(await _unitOfWork.artisanRequestRepository.GetArtisanRequestById(id));
            if(result == null || !result.Any())
            {
                return new Result<ViewRequestDTO>
                {
                    Error = 1,
                    Message = "Yêu cầu không được tìm thấy.",
                    Data = null
                };
            }
            return new Result<ViewRequestDTO>
            {
                Error = 0,
                Message = "Get Request successfully.",
                Data = result.FirstOrDefault()
            };
        }

        public async Task<Result<List<ViewRequestDTO>>> GetRequestByStatus(int pageIndex, int pageSize, RequestArtisanStatus status)
        {
            var result = _mapper.Map<List<ViewRequestDTO>>(await _unitOfWork.artisanRequestRepository.GetRequestByStatus(pageIndex, pageSize, status));
            if(result == null || !result.Any())
            {
                return new Result<List<ViewRequestDTO>>
                {
                    Error = 1,
                    Message = "No requests found for the given status.",
                    Data = null
                };
            }
            return new Result<List<ViewRequestDTO>>
            {
                Error = 0,
                Message = "Get Request by status successfully.",
                Data = result
            };
        }

        public async Task<Result<ViewRequestDTO>> SendRequestAsync(SendRequestDTO requestDto)
        {
            var request = _mapper.Map<ArtisanRequest>(requestDto);
            var image = await _cloudinaryService.UploadProductImage(requestDto.Image, FOLDER);
            request.Image = image.SecureUrl.ToString();
            await _unitOfWork.artisanRequestRepository.SendNewRequest(request);
            return new Result<ViewRequestDTO>
            {
                Error = 0,
                Message = "Yêu cầu gửi thành công.",
                Data = _mapper.Map<ViewRequestDTO>(request)
            };
        }

        public async Task<Result<object>> ApprovedRequest(Guid id)
        {
            var getRequest = await _unitOfWork.artisanRequestRepository.GetArtisanRequestById(id);
            var getUser = await _unitOfWork.userRepository.GetUserById(getRequest.UserId);
            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu không được tìm thấy.",
                    Data = null
                };
            }
            await _unitOfWork.artisanRequestRepository.AcceptRequest(getRequest);
            getUser.RoleId = 3;
            await _unitOfWork.userRepository.UpdateAsync(getUser);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 1,
                Message = "Chấp nhận yêu cầu thành công.",
                Data = null
            };
        }

        public async Task<Result<object>> CancelRequestByArtisan(Guid id)
        {
            var getRequest = await _unitOfWork.artisanRequestRepository.GetArtisanRequestById(id);
            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu không được tìm thấy.",
                    Data = null
                };
            }
            await _unitOfWork.artisanRequestRepository.CancelRequestByArtisan(getRequest);
            return new Result<object>
            {
                Error = 1,
                Message = "Hủy bỏ yêu cầu thành công.",
                Data = null
            };
        }

        public async Task<Result<object>> RejectedRequest(RejectRequestDTO rejectRequestDTO)
        {
            var getRequest = await _unitOfWork.artisanRequestRepository.GetArtisanRequestById(rejectRequestDTO.Id);
            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu không được tìm thấy.",
                    Data = null
                };
            }
            await _unitOfWork.artisanRequestRepository.RejectRequest(getRequest);
            getRequest.Reason = rejectRequestDTO.Reason;
            _unitOfWork.artisanRequestRepository.Update(getRequest);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 1,
                Message = "Từ chối yêu cầu thành công.",
                Data = null
            };
        }
    }
}
