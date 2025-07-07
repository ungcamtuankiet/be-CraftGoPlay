using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserAddress;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IUserService
    {
        Task<IList<ApplicationUser>> GetALl();
        Task<Result<List<UserDTO>>> GetAllAccountByStatusAsync(int pageIndex, int pageSize, StatusEnum status);
        Task<Result<object>> CreateNewAccountAsync(CreateNewAccountDTO createNewAccountDTO);
        Task<ApplicationUser> GetByEmail(string email);
        Task UpdateUserAsync(ApplicationUser user);
        Task<UserDTO> GetUserById(Guid id);
        Task<Result<UserDTO>> GetCurrentUserById();
        Task<Result<List<ViewAddressDTO>>> GetListAddressByUserId(Guid userId);
        Task<Result<object>> AddNewAddress(AddNewAddressDTO userAddress);
        Task<Result<object>> UpdateAddress(UpdateAddressDTO userAddress, Guid addressId);
        Task<Result<object>> DeleteAddress(Guid id);
    }
}
