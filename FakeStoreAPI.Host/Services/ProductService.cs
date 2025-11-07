using Microsoft.AspNetCore.Mvc;
using FakeStoreAPI.Host.Configuration;
using FakeStoreAPI.Host.Logging;
using System.Text.Json;
using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.DTOs.Internal;
using AutoMapper;

namespace FakeStoreAPI.Host.Services
{
    internal class ProductService : IProductService
    {
        #region Atributes
        private const string _className = "ProductService";
        private string _fakeStoreUrl = StoreAPIConfig.Get("FakeStoreUrl");
        private readonly IFakeStoreProductClient _productClient;
        private readonly IMapper _mapper;
        #endregion

        public ProductService(IFakeStoreProductClient productClient, IMapper mapper)
        {
            _productClient = productClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            try
            {
                var externalProducts = await _productClient.GetAllAsync();
                var products = _mapper.Map<IEnumerable<ProductDto>>(externalProducts);
                return products;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<ProductDto> GetByIdAsync(long id)
        {
            try
            {
                var externalProduct = await _productClient.GetByIdAsync(id);
                var product = _mapper.Map<ProductDto>(externalProduct);
                return product;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error: {ex.Message}");
                throw;
            }
        }
    }
}
