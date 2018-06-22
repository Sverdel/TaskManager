using System;
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
            CreateMap<ExchangeRate, ExchangeRateDto>().ForMember(dto => dto.Rate, opt => opt.MapFrom(rate => Math.Round(rate.Rate, 2))).ReverseMap();
        }
    }
}
