using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllAsync();
        public Task<ProductDto> GetByIdAsync(long id);
    }
}
