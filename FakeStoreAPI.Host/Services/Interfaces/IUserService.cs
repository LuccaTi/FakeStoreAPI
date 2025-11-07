using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllAsync();
        public Task<UserDto> GetByIdAsync(long id);
    }
}
