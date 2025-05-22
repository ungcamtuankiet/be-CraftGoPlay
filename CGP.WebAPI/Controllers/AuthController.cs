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
            // Chuyển hướng đến Google để xác thực
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
                return BadRequest("Invalid request data.");
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

        [HttpPost("register-google")]
        public async Task<IActionResult> RegisterWithGoogle([FromBody] GoogleUserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest("Invalid request data.");
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
        public async Task<IActionResult> RefreshToken(string token)
        {
            try
            {
                var checkRefeshToken = await _authService.RefreshToken(token);
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
                    Message = "Success",
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
        public async Task<IActionResult> Logout([FromBody] Guid userId)
        {
            Response.Cookies.Delete("refreshToken");
            await _authService.DeleteRefreshToken(userId);
            return Ok(new Result<object>
            {
                Error = 0,
                Message = "Logout Successfully",
                Data = null
            });
        }

        //Register user
        [HttpPost("user/register/user")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegistrationDTO userRegistrationDto)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "You are already logged in and cannot register again." });
            }

            try
            {
                await _authService.RegisterUserAsync(userRegistrationDto);
                return Ok(new { Message = "Registration successful. Please check your email for the OTP. " });
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
                    return BadRequest(new { Message = "Invalid OTP or OTP has expired." });
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

                return Ok(new { Message = "Email verified successfully." });
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
                    return Unauthorized(new { Message = "Invalid token." });
                }

                await _authService.ChangePasswordAsync(email, changePasswordDto);
                return Ok(new { Message = "Password changed successfully. Please log in again." });
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
                return Ok(new { Message = "Password reset link sent successfully." });
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
                return Ok(new { Message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
