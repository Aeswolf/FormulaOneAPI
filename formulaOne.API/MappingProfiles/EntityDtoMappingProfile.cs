using AutoMapper;
using formulaOne.API.DTOs.Requests;
using formulaOne.API.DTOs.Requests.Achievement;
using formulaOne.API.DTOs.Requests.Driver;
using formulaOne.API.DTOs.Responses;
using formulaOne.Entities.DbSets;

namespace formulaOne.API.MappingProfiles;

public class EntityDtoMappingProfile : Profile
{
    public EntityDtoMappingProfile()
    {
        CreateMap<Achievement, AchievementResponseDto>();
        CreateMap<CreateAchievementDto, Achievement>();
        CreateMap<UpdateAchievementDto, Achievement>();
        CreateMap<Driver, DriverResponseDto>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString()));
        CreateMap<CreateDriverDto, Driver>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth)));
        CreateMap<UpdateDriverDto, Driver>();
    }
}