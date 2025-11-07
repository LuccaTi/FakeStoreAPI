using AutoMapper;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<FakeStoreProductDto, ProductDto>();
        }
    }
}
