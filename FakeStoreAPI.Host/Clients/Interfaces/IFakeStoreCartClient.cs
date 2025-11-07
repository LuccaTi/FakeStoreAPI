using FakeStoreAPI.Host.DTOs.External;

namespace FakeStoreAPI.Host.Clients.Interfaces
{
    public interface IFakeStoreCartClient
    {
        public Task<IEnumerable<FakeStoreCartDto>> GetAllAsync();
        public Task<FakeStoreCartDto> GetByIdAsync(long id);
    }
}
