using CGP.Application.Interfaces;
using CGP.Contract.DTO.Auth;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CGP.Contract.DTO.User;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IGoogleService _googleService;
        private readonly HttpClient _httpClient;

        public AuthController(IAuthService authService, IUserService userService, IEmailService emailService, HttpClient httpClient, IGoogleService googleService)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
            _httpClient = httpClient;
            _googleService = googleService;
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDTO);
                Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Strict,
                });
                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Success",
                    Data = new
                    {
                        TokenType = "Bearer",
                        token.AccessToken,
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("get-url-google-login")]
        public async Task<IActionResult> GetUrlLoginGoogle()
        {
            var result = await _googleService.GetUrlLoginWithGoogle();
            return Ok(result);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackLoginGoogle([FromQuery] string code)
        {
            var result = await _googleService.GoogleCallback(code);
            return Content(result, "application/json");
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleUserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
            }

            try
            {
                var token = await _googleService.AuthenticateGoogleUserAsync(request);
                if (token.Code == 1)
                {
                    return BadRequest(token.Error);
                }
                Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Strict,
                });
                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Thành công",
                    Data = new
                    {
                        TokenType = "Bearer",
                        token.AccessToken,
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("register-google")]
        public async Task<IActionResult> RegisterWithGoogle([FromBody] GoogleUserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
            }

            try
            {
                var result = await _googleService.RegisterWithGoogle(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("token/refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var checkRefeshToken = await _authService.RefreshToken(request.RefreshToken);

                Response.Cookies.Append("refreshToken", checkRefeshToken.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Strict,
                });

                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Thành công",
                    Data = new
                    {
                        TokenType = "Bearer",
                        checkRefeshToken.AccessToken
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("user/logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody]Guid userId)
        {
            await _authService.DeleteRefreshToken(userId);
            Response.Cookies.Delete("refreshToken");
            return Ok(new Result<object>
            {
                Error = 0,
                Message = "Logout Successfully",
                Data = null
            });
        }



        /// <summary>
        /// Đăng ký tài khoản người dùng mới
        /// </summary>
        /// <remarks>
        /// ### Mô tả chi tiết:
        /// - API này cho phép đăng ký tài khoản người dùng mới trong hệ thống.
        /// - Sau khi đăng ký thành công, hệ thống sẽ gửi OTP xác thực qua email.
        /// - Người dùng cần xác thực OTP để kích hoạt tài khoản.
        /// 
        /// ### Yêu cầu dữ liệu:
        /// | Trường         | Kiểu dữ liệu | Ràng buộc                             | Ví dụ             |
        /// |----------------|--------------|---------------------------------------|-------------------|
        /// | UserName       | string       | 6-50 ký tự, không chứa ký tự đặc biệt | "user123"         |
        /// | Email          | string       | Định dạng email hợp lệ                | "user@email.com"  |
        /// | PhoneNo        | string       | Đúng 10 chữ số                        | "0987654321"      |
        /// | PasswordHash   | string       | 8-20 ký tự, gồm chữ hoa, thường, số và ký tự đặc biệt | "Pass@123" |
        /// 
        /// ### Ví dụ request:
        /// ```json
        /// {
        ///   "userName": "user123",
        ///   "email": "user@email.com",
        ///   "phoneNo": "0987654321",
        ///   "passwordHash": "Pass@123"
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">Đối tượng chứa thông tin đăng ký</param>
        /// <returns>Thông báo kết quả đăng ký</returns>
        /// <response code="201">Đăng ký thành công. OTP đã được gửi qua email</response>
        /// <response code="400">
        /// Các lỗi có thể xảy ra:
        /// - Đã đăng nhập
        /// - Dữ liệu không hợp lệ
        /// - Email/Username đã tồn tại
        /// </response>
        [HttpPost("user/register/user")]
        [SwaggerResponse(201, "Tạo thành công")]
        [SwaggerResponse(400, "Dữ liệu không hợp lệ")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDto)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "Bạn đã đăng nhập và không thể đăng ký lại." });
            }

            try
            {
                var result = await _authService.RegisterUserAsync(userRegistrationDto);
                if (result.Error == 1)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("user/otp/verify")]
        public async Task<IActionResult> VerifyOtp(OtpVerificationDTO otpVerificationDto)
        {
            try
            {
                var isValid = await _authService.VerifyOtpAndCompleteRegistrationAsync(otpVerificationDto.Email, otpVerificationDto.Otp);
                if (!isValid)
                {
                    return BadRequest(new { Message = "OTP không hợp lệ hoặc OTP đã hết hạn." });
                }

                var user = await _userService.GetByEmail(otpVerificationDto.Email);
                if (user != null)
                {
                    user.IsVerified = true;

                    if (user.RoleId == 3) // Role 3 is for shop
                    {
                        user.Status = StatusEnum.Pending;
                        await _emailService.SendPendingEmailAsync(user.Email);
                    }
                    else
                    {
                        user.Status = StatusEnum.Active;
                        await _emailService.SendApprovalEmailAsync(user.Email);
                    }

                    await _userService.UpdateUserAsync(user);
                }

                return Ok(new { Message = "Xác thực Email thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("user/otp/resend")]
        public async Task<IActionResult> ResendOtp([FromQuery] string email)
        {
            try
            {
                var result = await _authService.ResendOtpAsync(email);
                if (result.Error == 1)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        [HttpPost("user/password/change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    return Unauthorized(new { Message = "Token không hợp lệ." });
                }

                await _authService.ChangePasswordAsync(email, changePasswordDto);
                return Ok(new { Message = "Mật khẩu đã được thay đổi thành công, vui lòng đăng nhập lại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // <summary>
        /// Requests a password reset link to be sent to the user's email.
        /// </summary>
        [HttpPost("user/password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordRequestDTO forgotPasswordRequestDto)
        {
            try
            {
                await _authService.RequestPasswordResetAsync(forgotPasswordRequestDto);
                return Ok(new { Message = "Mã thông báo đặt lại mật khẩu đã được gửi thành công đến email của bạn." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        [HttpPost("user/password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                await _authService.ResetPasswordAsync(resetPasswordDto);
                return Ok(new { Message = "Đã đặt lại mật khẩu thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
