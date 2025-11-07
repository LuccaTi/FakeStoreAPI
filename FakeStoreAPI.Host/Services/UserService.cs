using AutoMapper;
using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Configuration;
using FakeStoreAPI.Host.DTOs.Internal;
using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Services
{
    public class UserService : IUserService
    {
        #region Atributes
        private const string _className = "UserService";
        private string _fakeStoreUrl = StoreAPIConfig.Get("FakeStoreUrl");
        private IFakeStoreUserClient _userClient;
        private IMapper _mapper;
        #endregion

        public UserService(IFakeStoreUserClient userClient, IMapper mapper)
        {
            _userClient = userClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                var externalUsers = await _userClient.GetAllAsync();
                var users = _mapper.Map<IEnumerable<UserDto>>(externalUsers);
                return users;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(long id)
        {
            try
            {
                var externalUser = await _userClient.GetByIdAsync(id);
                var user = _mapper.Map<UserDto>(externalUser);
                return user;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
