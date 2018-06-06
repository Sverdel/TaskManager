using AutoMapper;
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Models.Dto;
using TaskManager.Core.Model;

namespace TaskManager.Api.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ForMember(nameof(UserDto.Provider), opt => opt.MapFrom(nameof(User.LoginProvider))).ReverseMap();
            CreateMap<WorkTask, TaskDto>().ReverseMap();
            CreateMap<ExchangeRate, ExchangeRateDto>().ReverseMap();
        }
    }
}
