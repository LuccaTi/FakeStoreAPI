using FakeStoreAPI.Host.DTOs.External;

namespace FakeStoreAPI.Host.Clients.Interfaces
{
    public interface IFakeStoreUserClient
    {
        public Task<IEnumerable<FakeStoreUserDto>> GetAllAsync();
        public Task<FakeStoreUserDto> GetByIdAsync(long id);
    }
}
