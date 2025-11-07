using AutoMapper;
using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Configuration;
using FakeStoreAPI.Host.DTOs.Internal;
using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Services
{
    internal class CartService : ICartService
    {
        #region Atributes
        private const string _className = "CartService";
        private string _fakeStoreUrl = StoreAPIConfig.Get("FakeStoreUrl");
        private readonly IFakeStoreCartClient _cartClient;
        private readonly IMapper _mapper;
        #endregion

        public CartService(IFakeStoreCartClient cartClient, IMapper mapper)
        {
            _cartClient = cartClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            try
            {
                var externalCarts = await _cartClient.GetAllAsync();
                var carts = _mapper.Map<IEnumerable<CartDto>>(externalCarts);
                return carts;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<CartDto> GetByIdAsync(long id)
        {
            try
            {
                var externalCart = await _cartClient.GetByIdAsync(id);
                var cart = _mapper.Map<CartDto>(externalCart);
                return cart;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
