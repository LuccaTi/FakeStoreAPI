using AutoMapper;
using FakeStoreAPI.Host.DTOs.External;
using FakeStoreAPI.Host.DTOs.Internal;

namespace FakeStoreAPI.Host.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<FakeStoreUserDto, UserDto>();
            CreateMap<FakeStoreAPI.Host.DTOs.External.Address, FakeStoreAPI.Host.DTOs.Internal.Address>();
            CreateMap<FakeStoreAPI.Host.DTOs.External.Name, FakeStoreAPI.Host.DTOs.Internal.Name>();
        }
    }
}
