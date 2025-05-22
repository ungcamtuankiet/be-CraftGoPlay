using CGP.Contract.DTO.Auth;
using CGP.Contract.DTO.User;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetIdFromToken();
        //LOGIN
        Task<Authenticator> LoginAsync(LoginDTO loginDTO);
        Task<Authenticator> RefreshToken(string token);
        Task<bool> DeleteRefreshToken(Guid userId);

        //LOGIN GOOGLE
/*        Task<Authenticator> AuthenticateGoogleUserAsync(GoogleUserRequest request);*/

        //REGISTER
        Task RegisterUserAsync(UserRegistrationDTO userRegistrationDto);
        Task<ApplicationUser> GetByVerificationToken(string token);
        Task<bool> VerifyOtpAsync(string email, string otp);



        //Task RegisterShopAsync(ShopRegisterDTO shopRegisterDTO);
        Task<bool> VerifyOtpAndCompleteRegistrationAsync(string email, string otp);
        //Google
        /*        Task<Result<object>> UserCompleteSignUpByGoogle(SignupGoogleRequest userRegistrationDto);*/
        //CHANGE PASSWORD 
        Task ChangePasswordAsync(string email, ChangePasswordDTO changePasswordDto);

        //FORGOT PASSWORD
        Task RequestPasswordResetAsync(ForgotPasswordRequestDTO forgotPasswordRequestDto);
        Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
    }
}
