using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Services.Interfaces
{
    public interface ICartService
    {
        public Task<IEnumerable<CartDto>> GetAllAsync();
        public Task<CartDto> GetByIdAsync(long id);
    }
}
