using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Clients.Internal;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Clients
{
    public class FakeStoreUserClient : FakeStoreApiClientBase, IFakeStoreUserClient
    {
        #region Atributes
        private const string _className = "FakeStoreUserClient";
        #endregion
        public FakeStoreUserClient(HttpClient httpClient) : base(httpClient)
        {

        }
        public async Task<IEnumerable<FakeStoreUserDto>> GetAllAsync()
        {
            try
            {
                var users = await base.GetAllAsync<FakeStoreUserDto>("/users");
                return users;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<FakeStoreUserDto> GetByIdAsync(long id)
        {
            try
            {
                var user = await base.GetByIdAsync<FakeStoreUserDto>($"/users/{id}");
                return user ?? new FakeStoreUserDto();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
