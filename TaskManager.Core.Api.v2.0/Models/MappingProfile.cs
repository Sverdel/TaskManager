using AutoMapper;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.Core.Api.Models.Dto;
using TaskManager.TaskCore.Model;

namespace TaskManager.Core.Api.Models
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
