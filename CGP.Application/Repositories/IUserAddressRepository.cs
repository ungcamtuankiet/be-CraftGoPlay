using CGP.Domain.Entities;

namespace CGP.Application.Repositories
{
    public interface IUserAddressRepository : IGenericRepository<UserAddress>
    {
        public Task<List<UserAddress>> GetUserAddressesByUserId(Guid userId);
        public Task<UserAddress?> GetDefaultAddressByUserId(Guid userId);
        public Task<UserAddress?> GetUserAddressById(Guid addressId);
        public Task AddNewAddress(UserAddress userAddress);
        public Task UpdateAddress(UserAddress userAddress);
        public Task DeleteAddress(UserAddress userAddress);
    }
}
