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
        CreateMap<Driver, DriverResponseDto>();
        CreateMap<CreateDriverDto, Driver>();
        CreateMap<UpdateDriverDto, Driver>();
    }
}