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
                Message = "Lấy danh sách yêu cầu thành công.",
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
                Message = "Lấy yêu cầu thành công.",
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
                    Message = "Không có yêu cầu nào được tìm thấy với trạng thái yêu cầu đó.",
                    Data = null
                };
            }
            return new Result<List<ViewRequestDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách yêu cầu thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewRequestDTO>> SendRequestAsync(SendRequestDTO requestDto)
        {
            try
            {
                var existing = await _unitOfWork.artisanRequestRepository.GetPendingRequestByUserId(requestDto.UserId);
                var checkCraftVillage = await _unitOfWork.craftVillageRepository.GetByIdAsync(requestDto.CraftVillageId);
                var checkUser = await _unitOfWork.userRepository.GetByIdAsync(requestDto.UserId);
                var checkPhoneNo = await _unitOfWork.artisanRequestRepository.CheckPhoneNo(requestDto.PhoneNumber);
                if (existing != null)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Bạn đã gửi yêu cầu trước đó và đang chờ duyệt.",
                        Data = null
                    };
                }

                if(requestDto.Image == null || requestDto.Image.Length == 0)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Vui lòng tải lên hình ảnh.",
                        Data = null
                    };
                }

                if(checkCraftVillage == null)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Làng nghề không tồn tại.",
                        Data = null
                    };
                }

                if(checkUser == null)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Người dùng không tồn tại.",
                        Data = null
                    };
                }

                if(checkPhoneNo != null)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Số điện thoại đã được đăng kí trước đó.",
                        Data = null
                    };
                }

                if(requestDto.YearsOfExperience < 0)
                {
                    return new Result<ViewRequestDTO>
                    {
                        Error = 1,
                        Message = "Số năm kinh nghiệm không được nhỏ hơn 0.",
                        Data = null
                    };
                }

                var request = _mapper.Map<ArtisanRequest>(requestDto);

                var image = await _cloudinaryService.UploadProductImage(requestDto.Image, FOLDER);
                request.FullAddress = $"{requestDto.HomeNumber}, {requestDto.WardName}, {requestDto.DistrictName}, {requestDto.ProviceName}";
                request.Image = image.SecureUrl.ToString();
                if (requestDto.CraftIds != null && requestDto.CraftIds.Any())
                {
                    var craftSkills = await _unitOfWork.craftSkillRepository.GetByIdsAsyncs(requestDto.CraftIds);
                    request.CraftSkills = craftSkills;
                }
                await _unitOfWork.artisanRequestRepository.SendNewRequest(request);
                await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
                {
                    UserId = requestDto.UserId,
                    Action = "Gửi yêu cầu.",
                    EntityType = "Auth",
                    Description = "Bạn đã gửi yêu cầu trở thành nghệ nhân thành công.",
                    EntityId = request.Id,
                });
                return new Result<ViewRequestDTO>
                {
                    Error = 0,
                    Message = "Yêu cầu gửi thành công.",
                    Data = _mapper.Map<ViewRequestDTO>(request)
                };
            }
            catch (Exception ex)
            {
                return new Result<ViewRequestDTO>
                {
                    Error = 1,
                    Message = "Đã xảy ra lỗi khi tải lên hình ảnh: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ArtisanRequest?> GetPendingRequestByUserId(Guid userId)
        {
            return await _unitOfWork.artisanRequestRepository.GetPendingRequestByUserId(userId);
        }


        public async Task<Result<object>> ApprovedRequest(Guid id)
        {
            var getRequest = await _unitOfWork.artisanRequestRepository.GetArtisanRequestById(id);
            var getWallet = await _unitOfWork.walletRepository.GetWalletByUserIdAsync(getRequest.UserId);
            if (getRequest == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Yêu cầu không được tìm thấy.",
                    Data = null
                };
            }

            if (getRequest.Status != RequestArtisanStatus.Pending)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Chỉ có thể chấp nhận những yêu cầu đang chờ duyệt.",
                    Data = null
                };
            }

            var getUser = await _unitOfWork.userRepository.GetUserById(getRequest.UserId);
            getUser.CraftVillage_Id = getRequest.CraftVillageId;
            await _unitOfWork.artisanRequestRepository.AcceptRequest(getRequest);
            getUser.RoleId = 3;
            await _unitOfWork.userRepository.UpdateAsync(getUser);
            getWallet.Type = WalletTypeEnum.Artisan;
            _unitOfWork.walletRepository.Update(getWallet);

            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Chấp nhận yêu cầu thành công.",
                Data = null
            };
        }

        public async Task<Result<object>> CancelRequestByArtisan(Guid id)
        {
            var getRequest = await _unitOfWork.artisanRequestRepository.GetPendingRequestByUserId(id);
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

            await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = getRequest.UserId,
                Action = "Gửi yêu cầu.",
                EntityType = "Auth",
                Description = "Bạn đã hủy yêu cầu trở thành nghệ nhân thành công.",
                EntityId = id,
            });

            return new Result<object>
            {
                Error = 0,
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
                Error = 0,
                Message = "Từ chối yêu cầu thành công.",
                Data = null
            };
        }

        public async Task<Result<ViewRequestDTO>> GetLatestRequestByUserId(Guid userId)
        {
            var request = await _unitOfWork.artisanRequestRepository
                .GetLatestRequestByUserId(userId);

            if (request == null)
            {
                return new Result<ViewRequestDTO>
                {
                    Error = 1,
                    Message = "Người dùng chưa gửi yêu cầu nào.",
                    Data = null
                };
            }

            return new Result<ViewRequestDTO>
            {
                Error = 0,
                Message = "Lấy yêu cầu thành công.",
                Data = _mapper.Map<ViewRequestDTO>(request)
            };
        }

        public async Task<Result<object>> ResendRequest(Guid userId, Guid requestId)
        {
            var request = await _unitOfWork.artisanRequestRepository
                .GetRequestByIdAndUserId(requestId, userId);

            if (request == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy yêu cầu phù hợp để gửi lại.",
                    Data = null
                };
            }

            if (request.Status != RequestArtisanStatus.Cancelled)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Chỉ có thể gửi lại yêu cầu đã bị huỷ.",
                    Data = null
                };
            }

            request.Status = RequestArtisanStatus.Pending;
            _unitOfWork.artisanRequestRepository.Update(request);

            await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = userId,
                Action = "Gửi yêu cầu.",
                EntityType = "Auth",
                Description = "Bạn đã gửi lại yêu cầu trở thành nghệ nhân thành công.",
                EntityId = requestId,
            });

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Gửi lại yêu cầu thành công.",
                Data = null
            };
        }


    }
}
