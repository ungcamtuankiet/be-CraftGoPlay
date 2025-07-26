using CGP.Contract.Abstractions.Shared;
using CGP.Contract.DTO.ActivityLog;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserAddress;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;

namespace CGP.Application.Interfaces
{
    public interface IUserService
    {
        Task<IList<ApplicationUser>> GetALl();
        Task<List<ViewActivityDTO>> ViewActivityDTOs(Guid userId, int pageIndex, int pageSize);
        Task<AccountResponse<List<UserDTO>>> GetAllAccountByStatusAsync(int pageIndex, int pageSize, StatusEnum status);
        Task<Result<object>> CreateNewAccountAsync(CreateNewAccountDTO createNewAccountDTO);
        Task<Result<object>> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO);
        Task<Result<object>> DeleteAccountAsync(Guid id);
        Task<ApplicationUser> GetByEmail(string email);
        Task UpdateUserAsync(ApplicationUser user);
        Task<UserDTO> GetUserById(Guid id);
        Task<Result<UserDTO>> GetCurrentUserById();
        Task<Result<List<ViewAddressDTO>>> GetListAddressByUserId(Guid userId);
        Task<Result<ViewAddressDTO>> GetDefaultAddressByUserId(Guid userId);
        Task<Result<object>> AddNewAddress(AddNewAddressDTO userAddress);
        Task<Result<object>> UpdateAddress(UpdateAddressDTO userAddress, Guid addressId);
        Task<Result<object>> SetDefaultAddress(Guid addressId);
        Task<Result<object>> DeleteAddress(Guid id);
        Task<Result<object>> UpdateUserInfoAsync(UpdateInfoUserDTO updateDto);
    }
}
