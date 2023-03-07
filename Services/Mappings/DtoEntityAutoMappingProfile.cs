using AutoMapper;
using Dtos;
using Dtos.InputDtos;
using Entities.Entities;

namespace Services.Mappings
{
    public class DtoEntityAutoMappingProfile : Profile
    {
        public DtoEntityAutoMappingProfile()
        {
            RegisterEntityToDtoMapping();
            RegisterDtoToEntityMapping();
        }

        private void RegisterEntityToDtoMapping()
        {
            CreateMap<User, UserDto>();
        }

        private void RegisterDtoToEntityMapping()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
