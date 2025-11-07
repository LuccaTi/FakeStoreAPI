using FakeStoreAPI.Host.DTOs.External;

namespace FakeStoreAPI.Host.Clients.Interfaces
{
    public interface IFakeStoreProductClient
    {
        public Task<IEnumerable<FakeStoreProductDto>> GetAllAsync();
        public Task<FakeStoreProductDto> GetByIdAsync(long id);
    }
}
