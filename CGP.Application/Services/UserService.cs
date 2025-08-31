using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Utils;
using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.ActivityLog;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserAddress;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly IEmailService _emailService;
        private readonly TokenGenerators _tokenGenerators;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisService _redisService;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "thumbnail";

        public UserService(IConfiguration configuration,
            IAuthRepository authRepository, IEmailService emailService, IRedisService redisService,
            TokenGenerators tokenGenerators, IHttpContextAccessor httpContextAccessor, IClaimsService claimsService, IMapper mapper, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _emailService = emailService;
            _tokenGenerators = tokenGenerators;
            _httpContextAccessor = httpContextAccessor;
            _redisService = redisService;
            _claimsService = claimsService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IList<ApplicationUser>> GetALl()
        {
            var getUser = await _unitOfWork.userRepository.GetAllAsync();
            return getUser;
        }

        public async Task<List<ViewActivityDTO>> ViewActivityDTOs(Guid userId, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<ViewActivityDTO>>(await _unitOfWork.activityLogRepository.GetByUserIdAsync(userId, pageIndex, pageSize));
            return result;
        }

        public async Task<AccountResponse<List<UserDTO>>> GetAllAccountByStatusAsync(int pageIndex, int pageSize, StatusEnum status)
        {
            var getUser = _mapper.Map<List<UserDTO>>(await _unitOfWork.userRepository.GetAllAccountByStatusAsync(pageIndex, pageSize, status));
            return new AccountResponse<List<UserDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách người dùng thành công.",
                Count = getUser.Count,
                Data = getUser
            };
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await _unitOfWork.userRepository.GetUserByEmail(email);
        }

        public async Task<UserDTO> GetUserById(Guid id)
        {
            var user = await _unitOfWork.userRepository.GetUserById(id);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại!");
            }

            UserDTO userDto = new()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            return userDto;
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _unitOfWork.userRepository.UpdateAsync(user);
        }

        public async Task<Result<UserDTO>> GetCurrentUserById()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (token == null)
                return new Result<UserDTO>() { Error = 1, Message = "Token not found", Data = null };

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return new Result<UserDTO>() { Error = 1, Message = "Invalid token", Data = null };
            var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            var result = _mapper.Map<UserDTO>(await _unitOfWork.userRepository.GetAllUserById(userId));
            return new Result<UserDTO>() { Error = 0, Message = "Lấy thông tin người dùng thành công", Data = result };
        }

        public async Task<Result<ArtisanDTO>> GetCurrentArtisanById(Guid artisanId)
        {
            var getArtisan = await _unitOfWork.userRepository.GetUserById(artisanId);
            if (getArtisan == null)
            {
                return new Result<ArtisanDTO>()
                {
                    Error = 1,
                    Message = "Danh sách địa chỉ trống.",
                    Data = null
                };
            }

            var result = _mapper.Map<ArtisanDTO>(await _unitOfWork.userRepository.GetUserById(artisanId));
            return new Result<ArtisanDTO>() 
            { 
                Error = 0, 
                Message = "Lấy thông tin nghệ nhân thành công", 
                Data = result 
            };
        }

        public async Task<Result<List<ViewAddressDTO>>> GetListAddressByUserId(Guid userId)
        {
            var getUser = await _unitOfWork.userRepository.GetUserById(userId);
            if (getUser == null)
            {
                return new Result<List<ViewAddressDTO>>()
                {
                    Error = 1,
                    Message = "Danh sách địa chỉ trống.",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewAddressDTO>>(await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId));

            return new Result<List<ViewAddressDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách địa chỉ của người dùng thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewAddressDTO>> GetDefaultAddressByUserId(Guid userId)
        {
            var getUser = await _unitOfWork.userRepository.GetUserById(userId);
            if (getUser == null)
            {
                return new Result<ViewAddressDTO>()
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            var defaultAddress = await _unitOfWork.userAddressRepository.GetDefaultAddressByUserId(userId);
            if (defaultAddress == null)
            {
                return new Result<ViewAddressDTO>()
                {
                    Error = 1,
                    Message = "Không tìm thấy địa chỉ mặc định.",
                    Data = null
                };
            }

            var result = _mapper.Map<ViewAddressDTO>(defaultAddress);
            return new Result<ViewAddressDTO>()
            {
                Error = 0,
                Message = "Lấy địa chỉ mặc định thành công.",
                Data = result
            };
        }

        public async Task<Result<ViewAddressDTO>> GetAddressById(Guid addressId)
        {
            var address = await _unitOfWork.userAddressRepository.GetUserAddressById(addressId);
            if (address == null)
            {
                return new Result<ViewAddressDTO>
                {
                    Error = 1,
                    Message = "Địa chỉ không tồn tại.",
                    Data = null
                };
            }

            var result = _mapper.Map<ViewAddressDTO>(address);
            return new Result<ViewAddressDTO>
            {
                Error = 0,
                Message = "Lấy địa chỉ thành công.",
                Data = result
            };
        }

        public async Task<Result<object>> AddNewAddress(AddNewAddressDTO userAddress)
        {
            var address = _mapper.Map<UserAddress>(userAddress);
            address.FullAddress = $"{address.HomeNumber}, {address.WardName}, {address.DistrictName}, {address.ProviceName}";

            // If the new address is set as default, reset other addresses' IsDefault to false
            if (address.IsDefault)
            {
                var userAddresses = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(address.UserId);
                foreach (var existingAddress in userAddresses)
                {
                    if (existingAddress.IsDefault)
                    {
                        existingAddress.IsDefault = false;
                        await _unitOfWork.userAddressRepository.UpdateAddress(existingAddress);
                    }
                }
            }

            await _unitOfWork.userAddressRepository.AddNewAddress(address);
            return new Result<object>()
            {
                Error = 0,
                Message = "Thêm địa chỉ thành công.",
                Data = _mapper.Map<ViewAddressDTO>(address)
            };
        }

        public async Task<Result<object>> UpdateAddress(UpdateAddressDTO userAddress, Guid addressId)
        {
            var getUser = await _unitOfWork.userAddressRepository.GetByIdAsync(addressId);
            if (getUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Địa chỉ không tồn tại!",
                    Data = null
                };
            }
            getUser.FullAddress = $"{userAddress.HomeNumber}, {userAddress.WardName}, {userAddress.DistrictName}, {userAddress.ProviceName}";
            getUser.UserId = getUser.UserId;
            _mapper.Map(userAddress, getUser);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật địa chỉ thành công.",
                Data = _mapper.Map<ViewAddressDTO>(getUser)
            };
        }

        public async Task<Result<object>> SetDefaultAddress(Guid addressId)
        {
            var address = await _unitOfWork.userAddressRepository.GetByIdAsync(addressId);
            if (address == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Địa chỉ không tồn tại!",
                    Data = null
                };
            }

            // Lấy tất cả địa chỉ của người dùng và đặt IsDefault = false
            var userAddresses = await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(address.UserId);
            foreach (var userAddress in userAddresses)
            {
                if (userAddress.Id != addressId && userAddress.IsDefault)
                {
                    userAddress.IsDefault = false;
                    await _unitOfWork.userAddressRepository.UpdateAddress(userAddress);
                }
            }

            // Đặt địa chỉ được chọn thành IsDefault = true
            address.IsDefault = true;
            await _unitOfWork.userAddressRepository.UpdateAddress(address);

            return new Result<object>
            {
                Error = 0,
                Message = "Đặt địa chỉ mặc định thành công.",
                Data = _mapper.Map<ViewAddressDTO>(address)
            };
        }

        public async Task<Result<object>> DeleteAddress(Guid id)
        {
            var getUser = await _unitOfWork.userAddressRepository.GetByIdAsync(id);
            if (getUser == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Địa chỉ không tồn tại!",
                    Data = null
                };
            }
            await _unitOfWork.userAddressRepository.DeleteAddress(getUser);
            return new Result<object>()
            {
                Error = 0,
                Message = "Xóa địa chỉ thành công.",
                Data = null
            };
        }

        public async Task<Result<ViewAddressOfArtisanDTO>> GetAddressOfArtisan(Guid artisanId)
        {
            var artisanRequest = await _unitOfWork.artisanRequestRepository.GetLatestRequestByUserId(artisanId);
            if (artisanRequest == null || artisanRequest.Status != RequestArtisanStatus.Approved)
            {
                return new Result<ViewAddressOfArtisanDTO>
                {
                    Error = 1,
                    Message = "Người dùng không phải là thợ thủ công hoặc chưa được phê duyệt.",
                    Data = null
                };
            }

            var address = _mapper.Map<ViewAddressOfArtisanDTO>(artisanRequest);
            return new Result<ViewAddressOfArtisanDTO>
            {
                Error = 0,
                Message = "Lấy địa chỉ nghệ nhân thành công.",
                Data = address
            };
        }

        public async Task<Result<object>> SendRequestUpgradeToArtisan()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<object>> CreateNewAccountAsync(CreateNewAccountDTO createNewAccountDTO)
        {
            var user = _mapper.Map<ApplicationUser>(createNewAccountDTO);
            var checkEmail = await _unitOfWork.userRepository.GetUserByEmail(createNewAccountDTO.Email);
            if (checkEmail != null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Email đã tồn tại.",
                    Data = null
                };
            }

            var checkPhoneNo = await _unitOfWork.userRepository.FindByEmail(createNewAccountDTO.PhoneNumber);
            if (checkPhoneNo != null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Số điện thoại đã tồn tại.",
                    Data = null
                };
            }

            user.PasswordHash = HashPassword(createNewAccountDTO.PasswordHash);
            var uploadResult = await _cloudinaryService.UploadProductImage(createNewAccountDTO.Thumbnail, FOLDER);
            user.Thumbnail = uploadResult.SecureUrl.ToString();
            user.RoleId = 2;
            await _unitOfWork.userRepository.AddAsync(user);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Tạo tài khoản thành công.",
                Data = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<Result<object>> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO)
        {
            var getUser = await _unitOfWork.userRepository.GetUserById(updateAccountDTO.Id);
            if (getUser == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }

            // Kiểm tra Email trùng (trừ chính user này)
            if (!string.IsNullOrWhiteSpace(updateAccountDTO.Email) && updateAccountDTO.Email != getUser.Email)
            {
                var checkEmail = await _unitOfWork.userRepository.GetUserByEmail(updateAccountDTO.Email);
                if (checkEmail != null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Email đã tồn tại.",
                        Data = null
                    };
                }
                getUser.Email = updateAccountDTO.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateAccountDTO.PhoneNumber) && updateAccountDTO.PhoneNumber != getUser.PhoneNumber)
            {
                var checkPhone = await _unitOfWork.userRepository.FindByPhoneNoAsync(updateAccountDTO.PhoneNumber); // <-- Hàm đúng
                if (checkPhone != null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Số điện thoại đã tồn tại.",
                        Data = null
                    };
                }
                getUser.PhoneNumber = updateAccountDTO.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(updateAccountDTO.UserName))
                getUser.UserName = updateAccountDTO.UserName;

            if (updateAccountDTO.DateOfBirth.HasValue)
                getUser.DateOfBirth = updateAccountDTO.DateOfBirth;

            if (updateAccountDTO.Status != getUser.Status)
                getUser.Status = updateAccountDTO.Status;

            if (updateAccountDTO.RoleId != default)
                getUser.RoleId = (int)updateAccountDTO.RoleId;

            if (!string.IsNullOrWhiteSpace(updateAccountDTO.PasswordHash))
                getUser.PasswordHash = HashPassword(updateAccountDTO.PasswordHash);

            if (updateAccountDTO.Thumbnail != null)
            {
                await _cloudinaryService.DeleteImageAsync(getUser.Thumbnail);
                var uploadResult = await _cloudinaryService.UploadProductImage(updateAccountDTO.Thumbnail, FOLDER);
                getUser.Thumbnail = uploadResult.SecureUrl.ToString();
            }

            await _unitOfWork.userRepository.UpdateAsync(getUser);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Cập nhật tài khoản thành công.",
                Data = _mapper.Map<UserDTO>(getUser)
            };
        }

        public async Task<Result<object>> DeleteAccountAsync(Guid id)
        {
            var getUser = await _unitOfWork.userRepository.GetUserById(id);
            if (getUser == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Người dùng không tồn tại.",
                    Data = null
                };
            }
            await _cloudinaryService.DeleteImageAsync(getUser.Thumbnail);
            _unitOfWork.userRepository.Remove(getUser);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Xóa tài khoản thành công.",
                Data = null
            };
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<Result<object>> UpdateUserInfoAsync(UpdateInfoUserDTO updateDto)
        {
            try
            {
                var user = await _unitOfWork.userRepository.GetUserById(updateDto.Id);

                if (user == null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Không tìm thấy người dùng.",
                        Data = null
                    };
                }

                user.UserName = updateDto.UserName ?? user.UserName;
                user.Email = updateDto.Email ?? user.Email;
                user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
                user.DateOfBirth = updateDto.DateOfBirth ?? user.DateOfBirth;

                // Upload ảnh mới nếu có
                if (updateDto.Thumbnail != null && updateDto.Thumbnail.Length > 0)
                {
                    var uploadResult = await _cloudinaryService.UploadProductImage(updateDto.Thumbnail, FOLDER);
                    user.Thumbnail = uploadResult.SecureUrl.ToString();
                }

                await _unitOfWork.userRepository.UpdateAsync(user);

                return new Result<object>
                {
                    Error = 0,
                    Message = "Cập nhật thông tin người dùng thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Có lỗi xảy ra: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
