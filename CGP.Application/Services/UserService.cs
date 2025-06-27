using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Utils;
using CGP.Contract.DTO.Product;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserAddress;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly IEmailService _emailService;
        private readonly TokenGenerators _tokenGenerators;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisService _redisService;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IConfiguration configuration,
            IAuthRepository authRepository, IEmailService emailService, IRedisService redisService,
            TokenGenerators tokenGenerators, IHttpContextAccessor httpContextAccessor, IClaimsService claimsService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _authRepository = authRepository;
            _emailService = emailService;
            _tokenGenerators = tokenGenerators;
            _httpContextAccessor = httpContextAccessor;
            _redisService = redisService;
            _claimsService = claimsService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<ApplicationUser>> GetALl()
        {
            var getUser = await _userRepository.GetAllAsync();
            return getUser;
        }
        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<UserDTO> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new Exception("User is not exist!");
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
            await _userRepository.UpdateAsync(user);
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
            var result = _mapper.Map<UserDTO>(await _userRepository.GetAllUserById(userId));
            return new Result<UserDTO>() { Error = 0, Message = "Get Information Successfully", Data = result };
        }

        public async Task<Result<List<ViewAddressDTO>>> GetListAddressByUserId(Guid userId)
        {
            var getUser = await _unitOfWork.userRepository.GetUserById(userId);
            if (getUser == null)
            {
                return new Result<List<ViewAddressDTO>>()
                {
                    Error = 1,
                    Message = "User not found",
                    Data = null
                };
            }

            var result = _mapper.Map<List<ViewAddressDTO>>(await _unitOfWork.userAddressRepository.GetUserAddressesByUserId(userId));

            return new Result<List<ViewAddressDTO>>()
            {
                Error = 0,
                Message = "Get List Address Successfully",
                Data = result
            };
        }

        public async Task<Result<object>> AddNewAddress(AddNewAddressDTO userAddress)
        {
            var address = _mapper.Map<UserAddress>(userAddress);

            await _unitOfWork.userAddressRepository.AddNewAddress(address);
            return new Result<object>()
            {
                Error = 0,
                Message = "Add new address successfully",
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
                    Message = "Address not found",
                    Data = null
                };
            }

            getUser.UserId = getUser.UserId;
            _mapper.Map(userAddress, getUser);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Update address successfully",
                Data = _mapper.Map<ViewAddressDTO>(getUser)
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
                    Message = "Address not found",
                    Data = null
                };
            }
            await _unitOfWork.userAddressRepository.DeleteAddress(getUser);
            return new Result<object>()
            {
                Error = 0,
                Message = "Delete address successfully",
                Data = null
            };
        }

        public Task<Result<object>> SendRequestUpgradeToArtisan()
        {
            throw new NotImplementedException();
        }
    }
}
