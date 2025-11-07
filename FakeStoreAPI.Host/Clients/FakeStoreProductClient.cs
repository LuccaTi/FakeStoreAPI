using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Clients.Internal;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Clients
{
    public class FakeStoreProductClient : FakeStoreApiClientBase, IFakeStoreProductClient
    {
        #region Atributes
        private const string _className = "FakeStoreProductClient";
        #endregion
        public FakeStoreProductClient(HttpClient httpClient) : base(httpClient)
        {

        }
        public async Task<IEnumerable<FakeStoreProductDto>> GetAllAsync()
        {
            try
            {
                var products = await base.GetAllAsync<FakeStoreProductDto>("/products");
                return products;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<FakeStoreProductDto> GetByIdAsync(long id)
        {
            try
            {
                var product = await base.GetByIdAsync<FakeStoreProductDto>($"/products/{id}");
                return product ?? new FakeStoreProductDto();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
