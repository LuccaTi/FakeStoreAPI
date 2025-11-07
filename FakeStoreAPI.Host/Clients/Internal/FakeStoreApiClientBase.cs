using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.Logging;
using System.Net.Http;

namespace FakeStoreAPI.Host.Clients.Internal
{
    public class FakeStoreApiClientBase
    {
        #region Atributes
        private const string _className = "FakeStoreApiClientBase";
        protected readonly HttpClient _httpClient;
        #endregion

        public FakeStoreApiClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T?> GetByIdAsync<T>(string endpoint)
        {
            try
            {
                Logger.Debug(_className, "GetByIdAsync", $"Calling endpoint: {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                Logger.Debug(_className, "GetAllAsync", $"Request Success!");
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }

        protected async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            try
            {
                Logger.Debug(_className, "GetAllAsync", $"Calling endpoint: {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<T>>();
                Logger.Debug(_className, "GetAllAsync", $"Request Success!");
                return result ?? new List<T>();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
