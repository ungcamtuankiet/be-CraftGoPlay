using AutoMapper;
using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Application.Utils;
using CGP.Contract.DTO.Auth;
using CGP.Contract.DTO.User;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CloudinaryDotNet.Actions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Point = CGP.Domain.Entities.Point;

namespace CGP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly TokenGenerators _tokenGenerators;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IOtpService _otpService;
        private readonly IRedisService _redisService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserQuestService _userQuestService;
        private static string FOLDER = "thumbnail";

        public AuthService(IAuthRepository authRepository, TokenGenerators tokenGenerators,
            IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,
            IEmailService emailService, IConfiguration configuration, IOtpService otpService,
            IMapper mapper, IRedisService redisService, ICloudinaryService cloudinaryService, IUnitOfWork unitOfWork, IUserQuestService userQuestService)
        {
            _authRepository = authRepository;
            _tokenGenerators = tokenGenerators;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
            _otpService = otpService;
            _mapper = mapper;
            _redisService = redisService;
            _cloudinaryService = cloudinaryService;
            _unitOfWork = unitOfWork;
            _userQuestService = userQuestService;
        }


        // NEVER USER REDIS HERE :)
        public async Task<Authenticator> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(loginDTO.Email);

                if (user == null)
                {
                    throw new KeyNotFoundException("Email sai hoặc tài khoản không tồn tại.");
                }

                if (!user.IsVerified)
                {
                    throw new InvalidOperationException("Tài khoản chưa được kích hoạt. Vui lòng xác nhận email.");
                }
                if (user.Status.ToString() != "Active")
                {
                    throw new InvalidOperationException("Tài khoản đã bị khóa. Vui lòng liên hệ với trang web để được giải quyết.");
                }
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.PasswordHash, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Mật khẩu sai.");
                }

                // Generate JWT token
                await _userQuestService.EnsureDailyQuestAsync(user.Id);
                var token = await GenerateJwtToken(user);

                await _unitOfWork.activityLogRepository.AddAsync(new ActivityLog
                {
                    UserId = user.Id,
                    Action = "Bạn đã đăng nhập vào hệ thống",
                    EntityType = "Auth",
                    Description = "Bạn đã đăng nhập vào hệ thống thành công.",
                    EntityId = user.Id,
                });
                await _unitOfWork.SaveChangeAsync();
                return token;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle cases where the user is not found
                throw new ApplicationException("Email sai hoặc tài khoản không tồn tại.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where the account is not verified
                throw new ApplicationException("Tài khoản chưa được kích hoạt. Vui lòng xác nhận email.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle cases where the password is invalid
                throw new ApplicationException("Mật khẩu sai.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Xảy ra lỗi trong quá trình đăng nhập.", ex);
            }
        }


        private async Task<ApplicationUser> GetOrCreateExternalLoginUser(string provider, string key, string email)
        {
            var user = await _userRepository.FindByLoginAsync(provider, key);

            if (user != null)
                return user;

            user = await _userRepository.FindByEmail(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    PasswordHash = null,
                    Status = StatusEnum.Active,
                    Otp = "",
                    RoleId = 2,
                    CreationDate = DateTime.Now,
                    Provider = provider,
                    ProviderKey = key,
                    OtpExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(10)
                };
                await _userRepository.AddAsync(user);
            }
            else if (user.Provider != provider || user.ProviderKey != key)
            {
                // Cập nhật thông tin đăng nhập từ Google nếu cần
                user.Provider = provider;
                user.ProviderKey = key;
                await _userRepository.UpdateAsync(user);
            }

            return user;
        }


        //Register User Account
        public async Task<Result<object>> RegisterUserAsync(UserRegistrationDTO userRegistrationDto)
        {
            try
            {
                if (await _userRepository.ExistsAsync(u => u.Email == userRegistrationDto.Email))
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Email đã tồn tại.",
                        Data = null
                    };
                }
                var otp = GenerateOtp();
                //var uploadResult = await _cloudinaryService.UploadProductImage(userRegistrationDto.Thumbnail, FOLDER);
                var user = new ApplicationUser
                {
                    UserName = userRegistrationDto.UserName,
                    Email = userRegistrationDto.Email,
                    PasswordHash = HashPassword(userRegistrationDto.PasswordHash),
                    PhoneNumber = userRegistrationDto.PhoneNo,
                    Status = StatusEnum.Pending,
                    Otp = otp,
                    RoleId = 4,
                    CreationDate = DateTime.Now.AddHours(7),
                    OtpExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(10)
                };

                await _userRepository.AddAsync(user);
                await _emailService.SendOtpEmailAsync(user.Email, otp);
                return new Result<object>
                {
                    Error = 0,
                    Message = "Đăng ký tài khoản thành công. Vui lòng kiểm tra email để lấy mã OPT. ",
                    Data = null
                };
            }
            catch (ArgumentNullException ex)
            {
                // Handle cases where required information is missing
                throw new ApplicationException("Vui lòng điền các thông tin cần thiết.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where an operation is invalid, such as duplicate user registration
                throw new ApplicationException("Thao tác không hợp lệ trong quá trình đăng ký người dùng.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Xảy ra lỗi trong quá trình đăng ký.", ex);
            }
        }


        public async Task<ApplicationUser> GetByVerificationToken(string token)
        {
            try
            {
                return await _userRepository.GetUserByVerificationToken(token);
            }
            catch (Exception ex)
            {
                // Handle potential exceptions such as token not found
                throw new ApplicationException("Đã xảy ra lỗi khi truy xuất người dùng bằng mã thông báo xác minh.", ex);
            }
        }

        private async Task<Authenticator> GenerateJwtToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()), // Ensuring UserId claim is added
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120), // Token expiration set to 120 minutes
            signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid().ToString();
            await _authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new Authenticator
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        //logout

        public async Task<bool> DeleteRefreshToken(Guid userId)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var expiry = jwtToken.ValidTo;

            await _redisService.AddToBlacklistAsync(token, expiry);

            return await _authRepository.DeleteRefreshToken(userId);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private string GenerateOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                rng.GetBytes(byteArray);
                var otp = BitConverter.ToUInt32(byteArray, 0) % 1000000;
                return otp.ToString("D6");
            }
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy người dùng.");
                }

                if (user.Otp != otp || user.OtpExpiryTime < DateTime.UtcNow.AddHours(7))
                {
                    return false;
                }

                user.IsVerified = true;
                user.Otp = null;
                user.OtpExpiryTime = null;
                user.Status = StatusEnum.Active; // Update status to Active

                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle cases where the user is not found
                throw new ApplicationException("Không tìm thấy người dùng để xác minh OTP.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Đã xảy ra lỗi khi xác minh OTP.", ex);
            }
        }

        public async Task<Result<object>> ResendOtpAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.userRepository
                    .FindByEmail(email);

                if (user == null)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Email không tồn tại.",
                        Data = null
                    };
                }

                if (user.IsVerified)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Tài khoản đã được xác minh.",
                        Data = null
                    };
                }

                string otp;
                if (user.Otp != null && user.OtpExpiryTime.HasValue && user.OtpExpiryTime > DateTime.UtcNow.AddHours(7))
                {
                    otp = user.Otp;
                }
                else
                {
                    otp = GenerateOtp();
                    user.Otp = otp;
                    user.OtpExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(10);

                    await _unitOfWork.userRepository.UpdateAsync(user);
                    await _unitOfWork.SaveChangeAsync();
                }
                await _emailService.SendOtpEmailAsync(email, otp);
                return new Result<object>()
                {
                    Error = 0,
                    Message = "Gửi lại mã xác minh tới email thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Đã xảy ra lỗi khi gửi lại mã xác minh.",
                    Data = null
                };
            }
        }

        public async Task<bool> VerifyOtpAndCompleteRegistrationAsync(string email, string otp)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || user.Otp != otp || user.OtpExpiryTime < DateTime.UtcNow.AddHours(7))
            {
                return false;
            }

            user.IsVerified = true;
            user.Status = user.RoleId == 2 ? StatusEnum.Pending : StatusEnum.Active;
            user.Otp = "";
            user.OtpExpiryTime = null;
            var wallet = new Wallet()
            {
                User_Id = user.Id,
                AvailableBalance = 0,
                Type = WalletTypeEnum.User
            };
            await _unitOfWork.walletRepository.AddAsync(wallet);

            var point = new Point()
            {
                UserId = user.Id,
                Amount = 0
            };
            await _unitOfWork.pointRepository.AddAsync(point);

            await _userRepository.UpdateAsync(user);
            return true;
        }

        //PASSWORD
        public async Task ChangePasswordAsync(string email, ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.PasswordHash))
                {
                    throw new ArgumentException("Mật khẩu sai.");
                }

                if (changePasswordDto.NewPassword == changePasswordDto.OldPassword)
                {
                    throw new InvalidOperationException("Mật khẩu mới không được phép giống với mật khẩu cũ.");
                }

                if (!ValidatePassword(changePasswordDto.NewPassword))
                {
                    throw new ArgumentException("Mật khẩu mới phải chứa ít nhất một chữ cái viết hoa và một ký tự đặc biệt.");
                }

                user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
                await _userRepository.UpdateAsync(user);
            }
            catch (ArgumentException ex)
            {
                // Handle cases where the provided password details are invalid
                throw new ApplicationException("Thay đổi mật khẩu không thành công do nhập dữ liệu không hợp lệ.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where the new password is the same as the old password
                throw new ApplicationException("Không thể thay đổi mật khẩu do hạn chế về mặt hoạt động.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Đã xảy ra lỗi khi thay đổi mật khẩu.", ex);
            }
        }
        public async Task<Authenticator> RefreshToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token), "Refresh token is missing");

            var checkRefreshToken = _tokenGenerators.ValidateRefreshToken(token);
            if (!checkRefreshToken)
                return null;

            var user = await _authRepository.GetRefreshToken(token);
            if (user == null)
                return null;

            if (user.Role == null)
                throw new Exception("User role is missing");

            List<Claim> claims = new()
    {
        new Claim("id", user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Role.RoleName ?? "")
    };

            var (accessToken, refreshToken) = _tokenGenerators.GenerateTokens(claims);

            await _authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new Authenticator
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private bool ValidatePassword(string password)
        {
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));
            bool isValidLength = password.Length >= 6;

            return hasUpperCase && hasSpecialChar && isValidLength;
        }

        public async Task RequestPasswordResetAsync(ForgotPasswordRequestDTO forgotPasswordRequestDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(forgotPasswordRequestDto.Email);

                if (user == null || !user.IsVerified)
                {
                    throw new KeyNotFoundException("Người dùng không tìm thấy hoặc chưa được kích hoạt.");
                }

                var token = GenerateResetToken();
                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(8);

                await _userRepository.UpdateAsync(user);

                //var resetLink = $"{_configuration["AppSettings:FrontendUrl"]}/reset-password?token={token}"; -- FRONT-END ONLY

                await _emailService.SendEmailAsync(new EmailDTO
                {
                    To = user.Email,
                    Subject = "Yêu cầu đặt lại mật khẩu",
                    //Body = $"Please reset your password by clicking on the following link: <a href='{resetLink}'>Reset Password</a>" -- FRONT-END ONLY

                    Body = @$"Mã token của bạn để đặt lại mật khẩu là: {token}"
                });
            }
            catch (KeyNotFoundException ex)
            {
                // Handle cases where the user is not found or not activated
                throw new ApplicationException("Yêu cầu đặt lại mật khẩu không thành công do không tìm thấy người dùng hoặc không được kích hoạt.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Đã xảy ra lỗi khi yêu cầu đặt lại mật khẩu.", ex);
            }
        }
        public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                var user = await _userRepository.GetUserByResetToken(resetPasswordDto.Token);

                if (user == null || user.ResetTokenExpiry < DateTime.UtcNow.AddHours(7))
                {
                    throw new ArgumentException("Invalid or expired token.");
                }

                if (!ValidatePassword(resetPasswordDto.NewPassword))
                {
                    throw new ArgumentException("Mật khẩu mới phải chứa ít nhất một chữ cái viết hoa, một ký tự đặc biệt và dài ít nhất 6 ký tự.");
                }

                user.PasswordHash = HashPassword(resetPasswordDto.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;

                await _userRepository.UpdateAsync(user);
            }
            catch (ArgumentException ex)
            {
                // Handle cases where the token is invalid or the new password does not meet requirements
                throw new ApplicationException("Đặt lại mật khẩu không thành công do nhập không hợp lệ.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Đã xảy ra lỗi khi đặt lại mật khẩu.", ex);
            }
        }
        public async Task<string> GetIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
                throw new Exception("Token not found");

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                throw new Exception("Invalid token");

            var userId = jwtToken.Claims.First(claim => claim.Type == "id").Value;

            return userId;
        }
        private string GenerateResetToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[32];
                rng.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }
        }
    }
}
