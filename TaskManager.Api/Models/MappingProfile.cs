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
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<WorkTask, TaskDto>().ReverseMap();
        }
    }
}
