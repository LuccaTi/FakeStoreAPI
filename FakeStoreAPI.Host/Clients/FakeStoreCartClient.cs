using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Clients.Internal;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Clients
{
    public class FakeStoreCartClient : FakeStoreApiClientBase, IFakeStoreCartClient
    {
        #region Atributes
        private const string _className = "FakeStoreCartClient";
        #endregion
        public FakeStoreCartClient(HttpClient httpClient) : base(httpClient)
        {

        }
        public async Task<IEnumerable<FakeStoreCartDto>> GetAllAsync()
        {
            try
            {
                var carts = await base.GetAllAsync<FakeStoreCartDto>("/carts");
                return carts;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<FakeStoreCartDto> GetByIdAsync(long id)
        {
            try
            {
                var cart = await base.GetByIdAsync<FakeStoreCartDto>($"/carts/{id}");
                return cart ?? new FakeStoreCartDto();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
