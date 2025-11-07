using AutoMapper;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Mappers
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<FakeStoreCartDto, CartDto>()
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => ParseDate(src.date)));

            CreateMap<FakeStoreAPI.Host.DTOs.External.Products, FakeStoreAPI.Host.DTOs.Internal.Products>();
        }

        private static DateTime? ParseDate(string? dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return null;

            return DateTime.Parse(dateString);
        }
    }
}
